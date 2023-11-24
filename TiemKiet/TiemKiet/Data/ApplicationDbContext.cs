using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TiemKiet.Helpers;
using TiemKiet.Models;

namespace TiemKiet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
            SeedUser(builder);
            SeedUserRole(builder);
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }

        public DbSet<Country>? Countries { get; set; }
        public DbSet<Province>? Provinces { get; set; }
        public DbSet<District>? Districts { get; set; }
        public DbSet<Branch>? Branches { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<BlogPost>? BlogPosts { get; set; }
        public DbSet<ImageModel>? ImageModel { get; set; }
        public DbSet<TransactionLog>? TransactionLog { get; set; }

        public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)
            {
                builder.Property(u => u.Id).UseIdentityColumn(Constants.ValueStartUser, 1);
            }
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<ApplicationRole>().HasData
            (
                new ApplicationRole() { Id = 1, Name = Constants.Roles.Admin, NormalizedName = Constants.Roles.Admin, ConcurrencyStamp = null },
                new ApplicationRole() { Id = 2, Name = Constants.Roles.Staff, NormalizedName = Constants.Roles.Staff, ConcurrencyStamp = null }

            );
        }

        private void SeedUserRole(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<long>>().HasData(
                new IdentityUserRole<long>
                {
                    UserId = 1000000001,
                    RoleId = 1 // 
                }
            );
        }

        private void SeedUser(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1000000001,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = "admin@tiemkiet.vn",
                    NormalizedEmail = "admin@tiemkiet.vn",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAECAsUeOByw0jsD4x7X0K9WQdxWV/RrvPBnHITnRzdbrhHKzmf35BZDPXJBcVjp5FIQ==", //Admin@123
                    SecurityStamp = "ZD5UZJQK6Q5W6N7O6RBRF6DB2Q2G2AIJ",
                    ConcurrencyStamp = "b19f1b24-5ac9-4c8d-9b7c-5e5d5f5cfb1e",
                    FullName = "Admin",
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            );
        }

    }
}