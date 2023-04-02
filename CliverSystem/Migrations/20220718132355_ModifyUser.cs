using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliverSystem.Migrations
{
    public partial class ModifyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UsedFor",
                table: "User",
                newName: "ExpectedEarnings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpectedEarnings",
                table: "User",
                newName: "UsedFor");

            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "User",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
