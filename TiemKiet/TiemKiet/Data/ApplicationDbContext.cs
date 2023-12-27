using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TiemKiet.Helpers;
using TiemKiet.Models;

namespace TiemKiet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, IdentityUserClaim<long>, ApplicationUserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
            SeedRoles(builder);
            SeedUser(builder);
            SeedUserRole(builder);
        }

        public DbSet<Country>? Countries { get; set; }
        public DbSet<Province>? Provinces { get; set; }
        public DbSet<District>? Districts { get; set; }
        public DbSet<Branch>? Branches { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<BlogPost>? BlogPosts { get; set; }
        public DbSet<ImageModel>? ImageModel { get; set; }
        public DbSet<TransactionLog>? TransactionLog { get; set; }
        public DbSet<Voucher>? Vouchers { get; set; }
        public DbSet<VoucherUser>? VoucherUsers { get; set; }
        public DbSet<ManagerVoucherLog> ManagerVoucherLogs { get; set; }
        public DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<VersionModel> VersionModel { get; set; }
        public DbSet<ProductHome> ProductHome { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

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
            builder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole
                {
                    UserId = 1000000001,
                    RoleId = 1
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
                    PasswordHash = "AQAAAAEAACcQAAAAEGjoTI1vP//MoGZ+MmaqcaQANpnEaNIA/mRu21K4RuOTb/Z536KxBT4tUEEdguWDMQ==", //Admin@123
                    SecurityStamp = "ZD5UZJQK6Q5W6N7O6RBRF6DB2Q2G2AIJ",
                    ConcurrencyStamp = "b19f1b24-5ac9-4c8d-9b7c-5e5d5f5cfb1e",
                    FullName = "Admin",
                    TwoFactorEnabled = false,
                    PhoneNumber = "0923425148",
                    PhoneNumberConfirmed= true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            );
        }
    }
}