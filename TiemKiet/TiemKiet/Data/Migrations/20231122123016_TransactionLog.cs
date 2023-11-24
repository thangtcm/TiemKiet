using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class TransactionLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageTitle",
                table: "BlogPosts");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemove",
                table: "BlogPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdate",
                table: "BlogPosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "BlogPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserIdCreate",
                table: "BlogPosts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "BlogPosts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdUpdate",
                table: "BlogPosts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishUpload = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdUpload = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageModel_AspNetUsers_UserIdUpload",
                        column: x => x.UserIdUpload,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdCustomer = table.Column<long>(type: "bigint", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    DiscountPrice = table.Column<double>(type: "float", nullable: false),
                    DateTimePayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdStaff = table.Column<long>(type: "bigint", nullable: true),
                    PointOld = table.Column<double>(type: "float", nullable: false),
                    PointNew = table.Column<double>(type: "float", nullable: false),
                    ScroreOld = table.Column<double>(type: "float", nullable: false),
                    ScroreNew = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionLog_AspNetUsers_UserIdCustomer",
                        column: x => x.UserIdCustomer,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionLog_AspNetUsers_UserIdStaff",
                        column: x => x.UserIdStaff,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_UserIdCreate",
                table: "BlogPosts",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_UserIdRemove",
                table: "BlogPosts",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_UserIdUpdate",
                table: "BlogPosts",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_ImageModel_UserIdUpload",
                table: "ImageModel",
                column: "UserIdUpload");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLog_UserIdCustomer",
                table: "TransactionLog",
                column: "UserIdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLog_UserIdStaff",
                table: "TransactionLog",
                column: "UserIdStaff");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserIdCreate",
                table: "BlogPosts",
                column: "UserIdCreate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserIdRemove",
                table: "BlogPosts",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserIdUpdate",
                table: "BlogPosts",
                column: "UserIdUpdate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserIdCreate",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserIdRemove",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_UserIdUpdate",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "ImageModel");

            migrationBuilder.DropTable(
                name: "TransactionLog");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_UserIdCreate",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_UserIdRemove",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_UserIdUpdate",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "DateRemove",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "UserIdCreate",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "UserIdUpdate",
                table: "BlogPosts");

            migrationBuilder.AddColumn<string>(
                name: "PageTitle",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
