using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class FixVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_AspNetUsers_UserIdCreate",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_AspNetUsers_UserIdRemove",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_UserIdCreate",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_UserIdRemove",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "DateRemove",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "UserIdCreate",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "Vouchers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Vouchers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemove",
                table: "Vouchers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "UserIdCreate",
                table: "Vouchers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "Vouchers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_UserIdCreate",
                table: "Vouchers",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_UserIdRemove",
                table: "Vouchers",
                column: "UserIdRemove");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_AspNetUsers_UserIdCreate",
                table: "Vouchers",
                column: "UserIdCreate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_AspNetUsers_UserIdRemove",
                table: "Vouchers",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
