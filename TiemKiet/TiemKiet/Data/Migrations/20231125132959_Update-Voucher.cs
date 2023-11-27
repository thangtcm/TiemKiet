using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class UpdateVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductImgId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<double>(type: "float", nullable: false),
                    MaxDiscountAmount = table.Column<double>(type: "float", nullable: false),
                    MinBillAmount = table.Column<double>(type: "float", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdCreate = table.Column<long>(type: "bigint", nullable: true),
                    DateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpdate = table.Column<long>(type: "bigint", nullable: true),
                    DateRemove = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_AspNetUsers_UserIdCreate",
                        column: x => x.UserIdCreate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vouchers_AspNetUsers_UserIdUpdate",
                        column: x => x.UserIdUpdate,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ManagerVoucherLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdGive = table.Column<long>(type: "bigint", nullable: true),
                    UserIdClaim = table.Column<long>(type: "bigint", nullable: true),
                    VoucherId = table.Column<int>(type: "int", nullable: true),
                    DateTimeGives = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReponseGive = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerVoucherLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagerVoucherLogs_AspNetUsers_UserIdClaim",
                        column: x => x.UserIdClaim,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ManagerVoucherLogs_AspNetUsers_UserIdGive",
                        column: x => x.UserIdGive,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ManagerVoucherLogs_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoucherUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherId = table.Column<int>(type: "int", nullable: true),
                    UserIdClaim = table.Column<long>(type: "bigint", nullable: true),
                    RedeemedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherUsers_AspNetUsers_UserIdClaim",
                        column: x => x.UserIdClaim,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoucherUsers_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductImgId",
                table: "Products",
                column: "ProductImgId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerVoucherLogs_UserIdClaim",
                table: "ManagerVoucherLogs",
                column: "UserIdClaim");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerVoucherLogs_UserIdGive",
                table: "ManagerVoucherLogs",
                column: "UserIdGive");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerVoucherLogs_VoucherId",
                table: "ManagerVoucherLogs",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_UserIdCreate",
                table: "Vouchers",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_UserIdUpdate",
                table: "Vouchers",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsers_UserIdClaim",
                table: "VoucherUsers",
                column: "UserIdClaim");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsers_VoucherId",
                table: "VoucherUsers",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ImageModel_ProductImgId",
                table: "Products",
                column: "ProductImgId",
                principalTable: "ImageModel",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ImageModel_ProductImgId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ManagerVoucherLogs");

            migrationBuilder.DropTable(
                name: "VoucherUsers");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductImgId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImgId",
                table: "Products");
        }
    }
}
