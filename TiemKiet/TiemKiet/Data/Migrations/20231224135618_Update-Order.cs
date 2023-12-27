using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class UpdateOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StaffId",
                table: "Order",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_StaffId",
                table: "Order",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_StaffId",
                table: "Order",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_StaffId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_StaffId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Order");
        }
    }
}
