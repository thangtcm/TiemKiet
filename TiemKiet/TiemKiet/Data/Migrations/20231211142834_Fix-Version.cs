using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class FixVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlVersion",
                table: "VersionModel",
                newName: "UrlIOSVersion");

            migrationBuilder.AddColumn<string>(
                name: "UrlAndroidVersion",
                table: "VersionModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlAndroidVersion",
                table: "VersionModel");

            migrationBuilder.RenameColumn(
                name: "UrlIOSVersion",
                table: "VersionModel",
                newName: "UrlVersion");
        }
    }
}
