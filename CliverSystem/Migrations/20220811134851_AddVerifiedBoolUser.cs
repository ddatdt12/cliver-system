using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliverSystem.Migrations
{
    public partial class AddVerifiedBoolUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActived",
                table: "User",
                newName: "IsVerified");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "User",
                newName: "IsActived");
        }
    }
}
