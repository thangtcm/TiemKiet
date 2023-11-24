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

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

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

            //Service
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IRoleService), typeof(RoleService));
            services.AddTransient(typeof(ICountryService), typeof(CountryService));
            services.AddTransient(typeof(IProvinceService), typeof(ProvinceService));
            services.AddTransient(typeof(IDistrictService), typeof(DistrictService));
            services.AddTransient(typeof(IImageService), typeof(ImageService));
            services.AddTransient(typeof(IBranchService), typeof(BranchService));
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
