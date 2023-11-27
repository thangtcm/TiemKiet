using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class FixAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpiryDays",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "Vouchers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoucherName",
                table: "Vouchers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1000000001L,
                columns: new[] { "PhoneNumber", "PhoneNumberConfirmed" },
                values: new object[] { "092342005148", true });

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_UserIdRemove",
                table: "Vouchers",
                column: "UserIdRemove");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_AspNetUsers_UserIdRemove",
                table: "Vouchers",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_AspNetUsers_UserIdRemove",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_UserIdRemove",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ExpiryDays",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "VoucherName",
                table: "Vouchers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1000000001L,
                columns: new[] { "PhoneNumber", "PhoneNumberConfirmed" },
                values: new object[] { null, false });
        }
    }
}
