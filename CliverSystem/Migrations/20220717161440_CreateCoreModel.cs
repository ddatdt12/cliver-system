using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliverSystem.Migrations
{
    public partial class CreateCoreModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryTime = table.Column<int>(type: "int", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: true),
                    CanDesignCustomized = table.Column<bool>(type: "bit", nullable: false),
                    CanContentUpload = table.Column<bool>(type: "bit", nullable: false),
                    IsResponsiveDesign = table.Column<bool>(type: "bit", nullable: false),
                    HasSourceCode = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfRevisions = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubcategoryId = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasOfferPackages = table.Column<bool>(type: "bit", nullable: false),
                    BasicPackageId = table.Column<int>(type: "int", nullable: true),
                    StandardPackageId = table.Column<int>(type: "int", nullable: true),
                    PremiumPackageId = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Package_BasicPackageId",
                        column: x => x.BasicPackageId,
                        principalTable: "Package",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Post_Package_PremiumPackageId",
                        column: x => x.PremiumPackageId,
                        principalTable: "Package",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Post_Package_StandardPackageId",
                        column: x => x.StandardPackageId,
                        principalTable: "Package",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Post_Subcategory_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Post_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Post_SubcategoryId",
                table: "Post",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategory_CategoryId",
                table: "Subcategory",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Subcategory");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
}
