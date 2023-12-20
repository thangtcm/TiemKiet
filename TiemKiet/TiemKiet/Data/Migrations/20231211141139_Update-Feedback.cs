using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class UpdateFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherType",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductMBDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductPriceUp",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackId",
                table: "ImageModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFeedback = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ApplicationUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VersionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    IsDeploy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageModel_FeedbackId",
                table: "ImageModel",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ApplicationUserId",
                table: "Feedback",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageModel_Feedback_FeedbackId",
                table: "ImageModel",
                column: "FeedbackId",
                principalTable: "Feedback",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageModel_Feedback_FeedbackId",
                table: "ImageModel");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "VersionModel");

            migrationBuilder.DropIndex(
                name: "IX_ImageModel_FeedbackId",
                table: "ImageModel");

            migrationBuilder.DropColumn(
                name: "VoucherType",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ProductMBDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPriceUp",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "ImageModel");
        }
    }
}
