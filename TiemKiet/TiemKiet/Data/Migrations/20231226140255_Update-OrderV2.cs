using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class UpdateOrderV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Upsize",
                table: "OrderDetail",
                newName: "UpSize");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Order",
                newName: "NoteShip");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePreparing",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "LatCustomer",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ListVoucher",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "LongCustomer",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePreparing",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "LatCustomer",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ListVoucher",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "LongCustomer",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "UpSize",
                table: "OrderDetail",
                newName: "Upsize");

            migrationBuilder.RenameColumn(
                name: "NoteShip",
                table: "Order",
                newName: "Content");
        }
    }
}
