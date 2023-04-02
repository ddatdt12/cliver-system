using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliverSystem.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Package_BasicPackageId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Package_PremiumPackageId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Package_StandardPackageId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_BasicPackageId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_PremiumPackageId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_StandardPackageId",
                table: "Post");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "2da0c54d-afc3-475d-830c-78cac920b4a4");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "48eb824c-dbed-4b74-b1f7-9b84a68ae45a");

            migrationBuilder.DropColumn(
                name: "BasicPackageId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PremiumPackageId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "StandardPackageId",
                table: "Post");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Post",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Package",
                type: "varchar(12)",
                nullable: false,
                defaultValue: "Basic");

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AvailableForWithdrawal", "CreatedAt", "Description", "Email", "ExpectedEarnings", "IsActived", "Name", "NetIncome", "Password", "PendingClearance", "Type", "UpdatedAt", "UsedForPurchases", "Withdrawn" },
                values: new object[] { "53f891d8-bd32-40cf-a30c-04f2d5ecf164", 0L, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "test@gmail.com", 0L, true, "Test 1", 0L, "123123123", 0L, 1, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0L });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AvailableForWithdrawal", "CreatedAt", "Description", "Email", "ExpectedEarnings", "IsActived", "Name", "NetIncome", "Password", "PendingClearance", "Type", "UpdatedAt", "UsedForPurchases", "Withdrawn" },
                values: new object[] { "fedb88e2-decb-45a2-a0f1-8edc92b0b918", 0L, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "admin@admin.com", 0L, true, "admin", 0L, "123123123", 0L, 0, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0L, 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Post_Title",
                table: "Post",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Package_PostId",
                table: "Package",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_Type",
                table: "Package",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Post_PostId",
                table: "Package",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_Post_PostId",
                table: "Package");

            migrationBuilder.DropIndex(
                name: "IX_Post_Title",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Package_PostId",
                table: "Package");

            migrationBuilder.DropIndex(
                name: "IX_Package_Type",
                table: "Package");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "53f891d8-bd32-40cf-a30c-04f2d5ecf164");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "fedb88e2-decb-45a2-a0f1-8edc92b0b918");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Package");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Post",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "BasicPackageId",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PremiumPackageId",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandardPackageId",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AvailableForWithdrawal", "CreatedAt", "Description", "Email", "ExpectedEarnings", "IsActived", "Name", "NetIncome", "Password", "PendingClearance", "Type", "UpdatedAt", "UsedForPurchases", "Withdrawn" },
                values: new object[] { "2da0c54d-afc3-475d-830c-78cac920b4a4", 0L, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7630), "", "test@gmail.com", 0L, true, "Test 1", 0L, "123123123", 0L, 1, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7660), 0L, 0L });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AvailableForWithdrawal", "CreatedAt", "Description", "Email", "ExpectedEarnings", "IsActived", "Name", "NetIncome", "Password", "PendingClearance", "Type", "UpdatedAt", "UsedForPurchases", "Withdrawn" },
                values: new object[] { "48eb824c-dbed-4b74-b1f7-9b84a68ae45a", 0L, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7665), "", "admin@admin.com", 0L, true, "admin", 0L, "123123123", 0L, 0, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7665), 0L, 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Post_BasicPackageId",
                table: "Post",
                column: "BasicPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PremiumPackageId",
                table: "Post",
                column: "PremiumPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_StandardPackageId",
                table: "Post",
                column: "StandardPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Package_BasicPackageId",
                table: "Post",
                column: "BasicPackageId",
                principalTable: "Package",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Package_PremiumPackageId",
                table: "Post",
                column: "PremiumPackageId",
                principalTable: "Package",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Package_StandardPackageId",
                table: "Post",
                column: "StandardPackageId",
                principalTable: "Package",
                principalColumn: "Id");
        }
    }
}
