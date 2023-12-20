using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class FIxProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ImageModel_ProductImgId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductImgId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImgId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ProductImg",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductHome",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    productHomeType = table.Column<int>(type: "int", nullable: false),
                    UserUpdateId = table.Column<long>(type: "bigint", nullable: false),
                    DatePublish = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductHome", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductHome_AspNetUsers_UserUpdateId",
                        column: x => x.UserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductHome_UserUpdateId",
                table: "ProductHome",
                column: "UserUpdateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductHome");

            migrationBuilder.DropColumn(
                name: "ProductImg",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductImgId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductImgId",
                table: "Products",
                column: "ProductImgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ImageModel_ProductImgId",
                table: "Products",
                column: "ProductImgId",
                principalTable: "ImageModel",
                principalColumn: "Id");
        }
    }
}
