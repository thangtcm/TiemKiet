using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiemKiet.Migrations
{
    public partial class UpdateDatabaseLogUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Provinces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemove",
                table: "Provinces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdate",
                table: "Provinces",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Provinces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserIdCreate",
                table: "Provinces",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "Provinces",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdUpdate",
                table: "Provinces",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemove",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserIdCreate",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdUpdate",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Districts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemove",
                table: "Districts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdate",
                table: "Districts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Districts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserIdCreate",
                table: "Districts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "Districts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdUpdate",
                table: "Districts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemove",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdate",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserIdCreate",
                table: "Countries",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "Countries",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdUpdate",
                table: "Countries",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "Branches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRemove",
                table: "Branches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdate",
                table: "Branches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Branches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserIdCreate",
                table: "Branches",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdRemove",
                table: "Branches",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserIdUpdate",
                table: "Branches",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_UserIdCreate",
                table: "Provinces",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_UserIdRemove",
                table: "Provinces",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_UserIdUpdate",
                table: "Provinces",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserIdCreate",
                table: "Products",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserIdRemove",
                table: "Products",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserIdUpdate",
                table: "Products",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_UserIdCreate",
                table: "Districts",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_UserIdRemove",
                table: "Districts",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_UserIdUpdate",
                table: "Districts",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UserIdCreate",
                table: "Countries",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UserIdRemove",
                table: "Countries",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UserIdUpdate",
                table: "Countries",
                column: "UserIdUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UserIdCreate",
                table: "Branches",
                column: "UserIdCreate");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UserIdRemove",
                table: "Branches",
                column: "UserIdRemove");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UserIdUpdate",
                table: "Branches",
                column: "UserIdUpdate");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_UserIdCreate",
                table: "Branches",
                column: "UserIdCreate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_UserIdRemove",
                table: "Branches",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_UserIdUpdate",
                table: "Branches",
                column: "UserIdUpdate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_UserIdCreate",
                table: "Countries",
                column: "UserIdCreate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_UserIdRemove",
                table: "Countries",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_UserIdUpdate",
                table: "Countries",
                column: "UserIdUpdate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_AspNetUsers_UserIdCreate",
                table: "Districts",
                column: "UserIdCreate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_AspNetUsers_UserIdRemove",
                table: "Districts",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_AspNetUsers_UserIdUpdate",
                table: "Districts",
                column: "UserIdUpdate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserIdCreate",
                table: "Products",
                column: "UserIdCreate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserIdRemove",
                table: "Products",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_UserIdUpdate",
                table: "Products",
                column: "UserIdUpdate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_AspNetUsers_UserIdCreate",
                table: "Provinces",
                column: "UserIdCreate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_AspNetUsers_UserIdRemove",
                table: "Provinces",
                column: "UserIdRemove",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Provinces_AspNetUsers_UserIdUpdate",
                table: "Provinces",
                column: "UserIdUpdate",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_UserIdCreate",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_UserIdRemove",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_UserIdUpdate",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_UserIdCreate",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_UserIdRemove",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_UserIdUpdate",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_AspNetUsers_UserIdCreate",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_AspNetUsers_UserIdRemove",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_AspNetUsers_UserIdUpdate",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserIdCreate",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserIdRemove",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_UserIdUpdate",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_AspNetUsers_UserIdCreate",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_AspNetUsers_UserIdRemove",
                table: "Provinces");

            migrationBuilder.DropForeignKey(
                name: "FK_Provinces_AspNetUsers_UserIdUpdate",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_UserIdCreate",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_UserIdRemove",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Provinces_UserIdUpdate",
                table: "Provinces");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserIdCreate",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserIdRemove",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserIdUpdate",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Districts_UserIdCreate",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_UserIdRemove",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_UserIdUpdate",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Countries_UserIdCreate",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_UserIdRemove",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_UserIdUpdate",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Branches_UserIdCreate",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_UserIdRemove",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_UserIdUpdate",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "DateRemove",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "UserIdCreate",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "UserIdUpdate",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DateRemove",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserIdCreate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserIdUpdate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "DateRemove",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "UserIdCreate",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "UserIdUpdate",
                table: "Districts");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DateRemove",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UserIdCreate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UserIdUpdate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DateRemove",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DateUpdate",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "UserIdCreate",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "UserIdRemove",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "UserIdUpdate",
                table: "Branches");
        }
    }
}
