using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliverSystem.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    NumDaysReturnMoney = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.NumDaysReturnMoney);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberKeys = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastMessageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    NetIncome = table.Column<long>(type: "bigint", nullable: false),
                    Withdrawn = table.Column<long>(type: "bigint", nullable: false),
                    UsedForPurchases = table.Column<long>(type: "bigint", nullable: false),
                    PendingClearance = table.Column<long>(type: "bigint", nullable: false),
                    AvailableForWithdrawal = table.Column<long>(type: "bigint", nullable: false),
                    ExpectedEarnings = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityCardImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "https://t4.ftcdn.net/jpg/02/23/50/73/360_F_223507349_F5RFU3kL6eMt5LijOaMbWLeHUTv165CB.jpg"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RatingAvg = table.Column<double>(type: "float", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: false),
                    SubcategoryId = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatingAvg = table.Column<double>(type: "float", nullable: false),
                    RatingCount = table.Column<int>(type: "int", nullable: false),
                    Document = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasOfferPackages = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Subcategory_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Post_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomMembers",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "varchar(36)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomMembers", x => new { x.RoomId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_RoomMembers_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomMembers_User_MemberId",
                        column: x => x.MemberId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedLists_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubcategoryId = table.Column<int>(type: "int", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Budget = table.Column<long>(type: "bigint", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoneAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequest_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequest_Subcategory_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ServiceRequest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    NumberOfRevisions = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(12)", nullable: false, defaultValue: "Basic"),
                    DeliveryDays = table.Column<int>(type: "int", nullable: false),
                    ExpirationDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecentPost",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(36)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentPost", x => new { x.UserId, x.PostId });
                    table.ForeignKey(
                        name: "FK_RecentPost_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecentPost_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SavedSellers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(36)", nullable: false),
                    SavedListId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedSellers", x => new { x.SavedListId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SavedSellers_SavedLists_SavedListId",
                        column: x => x.SavedListId,
                        principalTable: "SavedLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SavedSellers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SavedServices",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    SavedListId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedServices", x => new { x.SavedListId, x.PostId });
                    table.ForeignKey(
                        name: "FK_SavedServices_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavedServices_SavedLists_SavedListId",
                        column: x => x.SavedListId,
                        principalTable: "SavedLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "varchar(36)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    RepliedToMessageId = table.Column<int>(type: "int", nullable: true),
                    CustomPackageId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Message_RepliedToMessageId",
                        column: x => x.RepliedToMessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Package_CustomPackageId",
                        column: x => x.CustomPackageId,
                        principalTable: "Package",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Message_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueBy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuyerId = table.Column<string>(type: "varchar(36)", nullable: false),
                    RevisionTimes = table.Column<int>(type: "int", nullable: false),
                    LeftRevisionTimes = table.Column<int>(type: "int", nullable: false),
                    LockedMoney = table.Column<long>(type: "bigint", nullable: false),
                    SellerId = table.Column<string>(type: "varchar(36)", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_SellerId",
                        column: x => x.SellerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ResourceId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderHistory_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(36)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionHistory_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionHistory_Wallet_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, "https://picsum.photos/50", "Graphics & Design" },
                    { 2, "https://picsum.photos/50", "Digital Marketing" },
                    { 3, "https://picsum.photos/50", "Writing & Translation" },
                    { 4, "https://picsum.photos/50", "Video & Animation" },
                    { 5, "https://picsum.photos/50", "Music & Audio" },
                    { 6, "https://picsum.photos/50", "Programming & Tech" },
                    { 7, "https://picsum.photos/50", "Business" },
                    { 8, "https://picsum.photos/50", "Lifestyle" },
                    { 9, "https://picsum.photos/50", "Trending" }
                });

            migrationBuilder.InsertData(
                table: "Parameters",
                column: "NumDaysReturnMoney",
                value: 5);

            migrationBuilder.InsertData(
                table: "Wallet",
                columns: new[] { "Id", "AvailableForWithdrawal", "Balance", "ExpectedEarnings", "NetIncome", "PendingClearance", "UsedForPurchases", "Withdrawn" },
                values: new object[,]
                {
                    { 1, 0L, 0L, 0L, 0L, 0L, 0L, 0L },
                    { 2, 0L, 0L, 0L, 0L, 0L, 0L, 0L },
                    { 3, 0L, 0L, 0L, 0L, 0L, 0L, 0L },
                    { 4, 0L, 0L, 0L, 0L, 0L, 0L, 0L },
                    { 9, 0L, 0L, 0L, 0L, 0L, 0L, 0L },
                    { 10, 0L, 0L, 0L, 0L, 0L, 0L, 0L }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, 1, "https://picsum.photos/50", "Social Media Marketing" },
                    { 2, 1, "https://picsum.photos/50", "Social Media Advertising" },
                    { 3, 1, "https://picsum.photos/50", "Search Engine Optimization (SEO)" },
                    { 4, 1, "https://picsum.photos/50", "Local SEO" },
                    { 5, 1, "https://picsum.photos/50", "Marketing Strategy" },
                    { 6, 1, "https://picsum.photos/50", "Public Relations" },
                    { 7, 1, "https://picsum.photos/50", "Guest Posting" },
                    { 8, 1, "https://picsum.photos/50", "Video Marketing" },
                    { 9, 1, "https://picsum.photos/50", "Email Marketing" },
                    { 10, 1, "https://picsum.photos/50", "Web Analytics" },
                    { 11, 1, "https://picsum.photos/50", "Text Message Marketing" },
                    { 12, 1, "https://picsum.photos/50", "Crowdfunding" },
                    { 13, 1, "https://picsum.photos/50", "Marketing Advice" },
                    { 14, 1, "https://picsum.photos/50", "Search Engine Marketing (SEM)" },
                    { 15, 1, "https://picsum.photos/50", "Display Advertising" },
                    { 16, 1, "https://picsum.photos/50", "E-Commerce Marketing" },
                    { 17, 1, "https://picsum.photos/50", "Influencer Marketing" },
                    { 18, 1, "https://picsum.photos/50", "Community Management" },
                    { 19, 1, "https://picsum.photos/50", "Mobile App Marketing" },
                    { 20, 1, "https://picsum.photos/50", "Music Promotion" },
                    { 21, 1, "https://picsum.photos/50", "Book & eBook Marketing" },
                    { 22, 1, "https://picsum.photos/50", "Podcast Marketing" },
                    { 23, 1, "https://picsum.photos/50", "Affiliate Marketing" },
                    { 24, 1, "https://picsum.photos/50", "Other" },
                    { 25, 2, "https://picsum.photos/50", "Logo Design" },
                    { 26, 2, "https://picsum.photos/50", "Brand Style Guides" },
                    { 27, 2, "https://picsum.photos/50", "Fonts & TypographyNEW" },
                    { 28, 2, "https://picsum.photos/50", "Business Cards & Stationery" },
                    { 29, 2, "https://picsum.photos/50", "Game Art" },
                    { 30, 2, "https://picsum.photos/50", "Graphics for Streamers" },
                    { 31, 2, "https://picsum.photos/50", "Twitch Store" },
                    { 32, 2, "https://picsum.photos/50", "Illustration" },
                    { 33, 2, "https://picsum.photos/50", "NFT Art" },
                    { 34, 2, "https://picsum.photos/50", "Pattern Design" },
                    { 35, 2, "https://picsum.photos/50", "Portraits & Caricatures" },
                    { 36, 2, "https://picsum.photos/50", "Cartoons & Comics" },
                    { 37, 2, "https://picsum.photos/50", "Tattoo Design" },
                    { 38, 2, "https://picsum.photos/50", "Storyboards" },
                    { 39, 2, "https://picsum.photos/50", "Website Design" },
                    { 40, 2, "https://picsum.photos/50", "App Design" },
                    { 41, 2, "https://picsum.photos/50", "UX Design" },
                    { 42, 2, "https://picsum.photos/50", "Landing Page Design" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Icon", "Name" },
                values: new object[,]
                {
                    { 43, 2, "https://picsum.photos/50", "Icon Design" },
                    { 44, 2, "https://picsum.photos/50", "Social Media Design" },
                    { 45, 2, "https://picsum.photos/50", "Email Design" },
                    { 46, 2, "https://picsum.photos/50", "Web Banners" },
                    { 47, 2, "https://picsum.photos/50", "Signage Design" },
                    { 48, 2, "https://picsum.photos/50", "Packaging & Covers" },
                    { 49, 2, "https://picsum.photos/50", "Packaging & Label Design" },
                    { 50, 2, "https://picsum.photos/50", "Book Design" },
                    { 51, 2, "https://picsum.photos/50", "Album Cover Design" },
                    { 52, 2, "https://picsum.photos/50", "Podcast Cover Art" },
                    { 53, 2, "https://picsum.photos/50", "Car Wraps" },
                    { 54, 3, "https://picsum.photos/50", "Articles & Blog Posts" },
                    { 55, 3, "https://picsum.photos/50", "Translation" },
                    { 56, 3, "https://picsum.photos/50", "Proofreading & Editing" },
                    { 57, 3, "https://picsum.photos/50", "Resume Writing" },
                    { 58, 3, "https://picsum.photos/50", "Cover Letters" },
                    { 59, 3, "https://picsum.photos/50", "LinkedIn Profiles" },
                    { 60, 3, "https://picsum.photos/50", "Ad Copy" },
                    { 61, 3, "https://picsum.photos/50", "Sales Copy" },
                    { 62, 3, "https://picsum.photos/50", "Social Media Copy" },
                    { 63, 3, "https://picsum.photos/50", "Email Copy" },
                    { 64, 3, "https://picsum.photos/50", "Case Studies" },
                    { 65, 3, "https://picsum.photos/50", "Book & eBook Writing" },
                    { 66, 3, "https://picsum.photos/50", "Book Editing" },
                    { 67, 3, "https://picsum.photos/50", "Scriptwriting" },
                    { 68, 3, "https://picsum.photos/50", "Podcast Writing" },
                    { 69, 3, "https://picsum.photos/50", "Beta Reading" },
                    { 70, 3, "https://picsum.photos/50", "Creative Writing" },
                    { 71, 3, "https://picsum.photos/50", "Brand Voice & Tone" },
                    { 72, 3, "https://picsum.photos/50", "UX Writing" },
                    { 73, 3, "https://picsum.photos/50", "Speechwriting" },
                    { 74, 3, "https://picsum.photos/50", "eLearning Content Development" },
                    { 75, 3, "https://picsum.photos/50", "Technical Writing" },
                    { 76, 4, "https://picsum.photos/50", "Video Editing" },
                    { 77, 4, "https://picsum.photos/50", "Short Video Ads" },
                    { 78, 4, "https://picsum.photos/50", "Whiteboard & Animated Explainers" },
                    { 79, 4, "https://picsum.photos/50", "Character Animation" },
                    { 80, 4, "https://picsum.photos/50", "Lyric & Music Videos" },
                    { 81, 4, "https://picsum.photos/50", "Logo Animation" },
                    { 82, 4, "https://picsum.photos/50", "Intros & Outros" },
                    { 83, 4, "https://picsum.photos/50", "Visual Effects" },
                    { 84, 4, "https://picsum.photos/50", "Subtitles & Captions" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Icon", "Name" },
                values: new object[,]
                {
                    { 85, 4, "https://picsum.photos/50", "Spokesperson Videos" },
                    { 86, 4, "https://picsum.photos/50", "Unboxing Videos" },
                    { 87, 4, "https://picsum.photos/50", "Animated GIFs" },
                    { 88, 5, "https://picsum.photos/50", "Voice Over" },
                    { 89, 5, "https://picsum.photos/50", "Producers & Composers" },
                    { 90, 5, "https://picsum.photos/50", "Singers & Vocalists" },
                    { 91, 5, "https://picsum.photos/50", "Mixing & Mastering" },
                    { 92, 5, "https://picsum.photos/50", "Session Musicians" },
                    { 93, 5, "https://picsum.photos/50", "Online Music Lessons" },
                    { 94, 5, "https://picsum.photos/50", "Podcast Editing" },
                    { 95, 5, "https://picsum.photos/50", "Songwriters" },
                    { 96, 5, "https://picsum.photos/50", "Beat Making" },
                    { 97, 5, "https://picsum.photos/50", "Audiobook Production" },
                    { 98, 5, "https://picsum.photos/50", "Audio Ads Production" },
                    { 99, 5, "https://picsum.photos/50", "Sound Design" },
                    { 100, 6, "https://picsum.photos/50", "WordPress" },
                    { 101, 6, "https://picsum.photos/50", "Website Builders & CMS" },
                    { 102, 6, "https://picsum.photos/50", "Game Development" },
                    { 103, 6, "https://picsum.photos/50", "Development for Streamers" },
                    { 104, 6, "https://picsum.photos/50", "Web Programming" },
                    { 105, 6, "https://picsum.photos/50", "E-Commerce Development" },
                    { 106, 6, "https://picsum.photos/50", "Mobile Apps" },
                    { 107, 6, "https://picsum.photos/50", "Desktop Applications" },
                    { 108, 6, "https://picsum.photos/50", "Chatbots" },
                    { 109, 6, "https://picsum.photos/50", "Support & IT" },
                    { 110, 6, "https://picsum.photos/50", "Online Coding Lessons" },
                    { 111, 6, "https://picsum.photos/50", "Cybersecurity & Data Protection" },
                    { 112, 6, "https://picsum.photos/50", "Electronics Engineering" },
                    { 113, 6, "https://picsum.photos/50", "Convert Files" },
                    { 114, 6, "https://picsum.photos/50", "User Testing" },
                    { 115, 6, "https://picsum.photos/50", "QA & Review" },
                    { 116, 6, "https://picsum.photos/50", "Blockchain & Cryptocurrency" },
                    { 117, 6, "https://picsum.photos/50", "NFT Development" },
                    { 118, 6, "https://picsum.photos/50", "Databases" },
                    { 119, 6, "https://picsum.photos/50", "Data Processing" },
                    { 120, 6, "https://picsum.photos/50", "Data Engineering" },
                    { 121, 6, "https://picsum.photos/50", "Data Science" },
                    { 122, 6, "https://picsum.photos/50", "Other" },
                    { 123, 6, "https://picsum.photos/50", "" },
                    { 124, 7, "https://picsum.photos/50", "Virtual Assistant" },
                    { 125, 7, "https://picsum.photos/50", "E-Commerce Management" },
                    { 126, 7, "https://picsum.photos/50", "Market Research" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Icon", "Name" },
                values: new object[,]
                {
                    { 127, 7, "https://picsum.photos/50", "Sales" },
                    { 128, 7, "https://picsum.photos/50", "Customer Care" },
                    { 129, 7, "https://picsum.photos/50", "CRM Management NEW" },
                    { 130, 7, "https://picsum.photos/50", "ERP ManagementNEW" },
                    { 131, 7, "https://picsum.photos/50", "Supply Chain Management" },
                    { 132, 7, "https://picsum.photos/50", "Project Management" },
                    { 133, 7, "https://picsum.photos/50", "Event ManagementNEW" },
                    { 134, 7, "https://picsum.photos/50", "Game Concept Design" },
                    { 135, 7, "https://picsum.photos/50", "Business Plans" },
                    { 136, 7, "https://picsum.photos/50", "Financial Consulting" },
                    { 137, 7, "https://picsum.photos/50", "Legal Consulting" },
                    { 138, 7, "https://picsum.photos/50", "Business Consulting" },
                    { 139, 7, "https://picsum.photos/50", "Presentations" },
                    { 140, 7, "https://picsum.photos/50", "HR Consulting" },
                    { 141, 7, "https://picsum.photos/50", "Career Counseling" },
                    { 142, 7, "https://picsum.photos/50", "Data Entry" },
                    { 143, 7, "https://picsum.photos/50", "Data Analytics" },
                    { 144, 7, "https://picsum.photos/50", "Data Visualization" },
                    { 145, 7, "https://picsum.photos/50", "Other" },
                    { 146, 8, "https://picsum.photos/50", "Online Tutoring" },
                    { 147, 8, "https://picsum.photos/50", "Gaming" },
                    { 148, 8, "https://picsum.photos/50", "Astrology & Psychics" },
                    { 149, 8, "https://picsum.photos/50", "Modeling & Acting" },
                    { 150, 8, "https://picsum.photos/50", "Wellness" },
                    { 151, 8, "https://picsum.photos/50", "Traveling" },
                    { 152, 8, "https://picsum.photos/50", "Fitness Lessons" },
                    { 153, 8, "https://picsum.photos/50", "Dance Lessons" },
                    { 154, 8, "https://picsum.photos/50", "Life Coaching" },
                    { 155, 8, "https://picsum.photos/50", "Greeting Cards & Videos" },
                    { 156, 8, "https://picsum.photos/50", "Personal Stylists" },
                    { 157, 8, "https://picsum.photos/50", "Cooking Lessons" },
                    { 158, 8, "https://picsum.photos/50", "Craft Lessons" },
                    { 159, 8, "https://picsum.photos/50", "Arts & Crafts" },
                    { 160, 8, "https://picsum.photos/50", "Family & Genealogy" },
                    { 161, 8, "https://picsum.photos/50", "Collectibles" },
                    { 162, 8, "https://picsum.photos/50", "Other" },
                    { 163, 9, "https://picsum.photos/50", "Dropshipping" },
                    { 164, 9, "https://picsum.photos/50", "E-Commerce Marketing" },
                    { 165, 9, "https://picsum.photos/50", "Game Development" },
                    { 166, 9, "https://picsum.photos/50", "Discord Services" },
                    { 167, 9, "https://picsum.photos/50", "NFT Services" },
                    { 168, 9, "https://picsum.photos/50", "Architecture & Interior Design" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Icon", "Name" },
                values: new object[,]
                {
                    { 169, 9, "https://picsum.photos/50", "Data" },
                    { 170, 9, "https://picsum.photos/50", "Resume Writing" },
                    { 171, 9, "https://picsum.photos/50", "Search Engine Optimization (SEO)" },
                    { 172, 9, "https://picsum.photos/50", "Character Modeling" },
                    { 173, 9, "https://picsum.photos/50", "Character Animation" },
                    { 174, 9, "https://picsum.photos/50", "Image Editing" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Avatar", "CreatedAt", "Description", "Email", "IdentityCardImage", "IsActive", "IsVerified", "Languages", "Name", "Password", "RatingAvg", "RatingCount", "Skills", "Type", "UpdatedAt", "WalletId" },
                values: new object[,]
                {
                    { "19328465-fcf8-4315-b687-bba6b86d13ed", "https://picsum.photos/200", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "test5@gmail.com", null, false, true, "", "test 5", "123123123", 0.0, 0, "", 1, new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { "3dbb7902-74a5-4113-9052-a13919a73949", "https://picsum.photos/200", new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "test2@gmail.com", null, false, true, "", "Test 2", "123123123", 0.0, 0, "", 1, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { "53f891d8-bd32-40cf-a30c-04f2d5ecf164", "https://picsum.photos/200", new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "test@gmail.com", null, false, true, "", "Test 1", "123123123", 0.0, 0, "", 1, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { "733a44cc-f3b5-4e79-8dda-afb6be9c72a3", "https://picsum.photos/200", new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "test4@gmail.com", null, false, true, "", "test 4", "123123123", 0.0, 0, "", 1, new DateTime(2022, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { "f5a6e9d2-a322-4e0d-bf39-8acf7b6b2fc6", "https://picsum.photos/200", new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "test3@gmail.com", null, false, true, "", "Test 3", "123123123", 0.0, 0, "", 1, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { "fedb88e2-decb-45a2-a0f1-8edc92b0b918", "https://picsum.photos/200", new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "admin@admin.com", null, false, true, "", "admin", "123123123", 0.0, 0, "", 0, new DateTime(2022, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_CustomPackageId",
                table: "Message",
                column: "CustomPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_PostId",
                table: "Message",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RepliedToMessageId",
                table: "Message",
                column: "RepliedToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RoomId",
                table: "Message",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_BuyerId",
                table: "Order",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PackageId",
                table: "Order",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SellerId",
                table: "Order",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_OrderId",
                table: "OrderHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistory_ResourceId",
                table: "OrderHistory",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_PostId",
                table: "Package",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_Type",
                table: "Package",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Post_SubcategoryId",
                table: "Post",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Title",
                table: "Post",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentPost_PostId",
                table: "RecentPost",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_OrderId",
                table: "Review",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_MemberKeys",
                table: "Room",
                column: "MemberKeys",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_MemberId",
                table: "RoomMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedLists_UserId",
                table: "SavedLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedSellers_UserId",
                table: "SavedSellers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedServices_PostId",
                table: "SavedServices",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_CategoryId",
                table: "ServiceRequest",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_SubcategoryId",
                table: "ServiceRequest",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_UserId",
                table: "ServiceRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategory_CategoryId",
                table: "Subcategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_OrderId",
                table: "TransactionHistory",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_WalletId",
                table: "TransactionHistory",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_User_WalletId",
                table: "User",
                column: "WalletId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "OrderHistory");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "RecentPost");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "RoomMembers");

            migrationBuilder.DropTable(
                name: "SavedSellers");

            migrationBuilder.DropTable(
                name: "SavedServices");

            migrationBuilder.DropTable(
                name: "ServiceRequest");

            migrationBuilder.DropTable(
                name: "TransactionHistory");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "SavedLists");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Subcategory");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Wallet");
        }
    }
}
