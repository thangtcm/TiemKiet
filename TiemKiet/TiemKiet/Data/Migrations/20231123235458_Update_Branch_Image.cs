using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class Update_Branch_Image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ImageModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageModel_BranchId",
                table: "ImageModel",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageModel_Branches_BranchId",
                table: "ImageModel",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageModel_Branches_BranchId",
                table: "ImageModel");

            migrationBuilder.DropIndex(
                name: "IX_ImageModel_BranchId",
                table: "ImageModel");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ImageModel");
        }
    }
}
