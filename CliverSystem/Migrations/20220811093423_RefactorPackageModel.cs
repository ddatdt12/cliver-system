using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliverSystem.Migrations
{
    public partial class RefactorPackageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanContentUpload",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "CanDesignCustomized",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "HasSourceCode",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "IsResponsiveDesign",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "NumberOfPages",
                table: "Package");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "Package",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAvailable",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "CanContentUpload",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanDesignCustomized",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSourceCode",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsResponsiveDesign",
                table: "Package",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPages",
                table: "Package",
                type: "int",
                nullable: true);
        }
    }
}
