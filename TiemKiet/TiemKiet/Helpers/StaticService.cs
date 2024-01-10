using TiemKiet.Repository.Interface;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Repository;
using TiemKiet.Services.Interface;
using TiemKiet.Services;
using TiemKiet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;
using Firebase.Auth;
using TiemKiet.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace TiemKiet.Helpers
{
    public static class StaticService
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize);
            services.Configure<FirebaseConfigVM>(configuration.GetSection("FirebaseConfig"));
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedPhoneNumber = true)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultPhoneProvider;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            });
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(15));

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie.SameSite = SameSiteMode.None;
                options.LoginPath = "/Home";
                options.AccessDeniedPath = "/Home/Error";
                options.SlidingExpiration = true;
            });
            //Repository
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ITransactionLogRepository, TransactionLogRepository>();
            services.AddScoped<IManagerVoucherLogRepository, ManagerVoucherLogRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IVoucherUserRepository, VoucherUserRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IVersionRepository, VersionRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IProductHomeRepository, ProductHomeRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

            //Service
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IRoleService), typeof(RoleService));
            services.AddTransient(typeof(ICountryService), typeof(CountryService));
            services.AddTransient(typeof(IProvinceService), typeof(ProvinceService));
            services.AddTransient(typeof(IDistrictService), typeof(DistrictService));
            services.AddTransient(typeof(IImageService), typeof(ImageService));
            services.AddTransient(typeof(IBranchService), typeof(BranchService));
            services.AddTransient(typeof(IProductService), typeof(ProductService));
            services.AddTransient(typeof(ITranscationLogService), typeof(TranscationLogService));
            services.AddTransient(typeof(IManagerVoucherLogRepository), typeof(ManagerVoucherLogRepository));
            services.AddTransient(typeof(IVoucherService), typeof(VoucherService));
            services.AddTransient(typeof(IVoucherUserService), typeof(VoucherUserService));
            services.AddTransient(typeof(IUserTokenService), typeof(UserTokenService));
            services.AddTransient(typeof(IBlogService), typeof(BlogService));
            services.AddTransient(typeof(IFirebaseStorageService), typeof(FirebaseStorageService));
            services.AddTransient(typeof(IVersionService), typeof(VersionService));
            services.AddTransient(typeof(IProductHomeService), typeof(ProductHomeService));
            services.AddTransient(typeof(IBannerService), typeof(BannerService));
            services.AddTransient(typeof(IOrderService), typeof(OrderService));
            services.AddTransient(typeof(INotifyFCMService), typeof(NotifyFCMService));
            //services.AddTransient(typeof(IFeedbackService), typeof(FeedbackService));
            services.AddScoped<AuthorizeWithMessageFilter>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //Authentication
            AddAuthorizationPolicies(services);

            services.AddSession(options =>
            {
                 options.Cookie.Name = ".TiemKiet.Session";
                 options.IdleTimeout = TimeSpan.FromMinutes(30);
                 options.Cookie.IsEssential = true;
                 options.Cookie.HttpOnly = true;
            });
            //Console.WriteLine(JsonConvert.SerializeObject(configuration.GetSection("FirebaseConfig").Get<FirebaseNotiConfig>()) + "\n\n\n");
            try
            {
                
                var firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tiemkiet-firebase-admin.json")),
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating FirebaseApp: {ex.Message}");
            }

            services.AddHttpClient();
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Admin));
                options.AddPolicy(Constants.Policies.RequireStaff, policy => policy.RequireRole(Constants.Roles.Admin, Constants.Roles.Staff));
                options.AddPolicy(Constants.Policies.RequireStaffNGT, policy => policy.RequireRole(Constants.Roles.Admin, Constants.Roles.Staff, Constants.Roles.TiemKietNGT));
                options.AddPolicy(Constants.Policies.RequireStaffPNT, policy => policy.RequireRole(Constants.Roles.Admin, Constants.Roles.Staff, Constants.Roles.TiemKietPNT));
                options.AddPolicy(Constants.Policies.RequireStaffNB, policy => policy.RequireRole(Constants.Roles.Admin, Constants.Roles.Staff, Constants.Roles.TiemKietNB));
            });
        }
    }
}
