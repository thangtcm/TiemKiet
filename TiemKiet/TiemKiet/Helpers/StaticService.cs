using TiemKiet.Repository.Interface;
using TiemKiet.Repository.UnitOfWork;
using TiemKiet.Repository;
using TiemKiet.Services.Interface;
using TiemKiet.Services;
using TiemKiet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TiemKiet.Helpers
{
    public static class StaticService
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
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
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
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

            //Authentication
            AddAuthorizationPolicies(services);

            services.AddSession(options =>
            {
                 options.Cookie.Name = ".TiemKiet.Session";
                 options.IdleTimeout = TimeSpan.FromMinutes(30);
                 options.Cookie.IsEssential = true;
                 options.Cookie.HttpOnly = true;
            });
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Admin));
                options.AddPolicy(Constants.Policies.RequireStaff, policy => policy.RequireRole(Constants.Roles.Admin, Constants.Roles.Staff));
            });
        }
    }
}
