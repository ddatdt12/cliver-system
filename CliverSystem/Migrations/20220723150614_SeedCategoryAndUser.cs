using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliverSystem.Migrations
{
    public partial class SeedCategoryAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Graphics & Design" },
                    { 2, "Digital Marketing" },
                    { 3, "Writing & Translation" },
                    { 4, "Video & Animation" },
                    { 5, "Music & Audio" },
                    { 6, "Programming & Tech" },
                    { 7, "Business" },
                    { 8, "Lifestyle" },
                    { 9, "Trending" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AvailableForWithdrawal", "CreatedAt", "Description", "Email", "ExpectedEarnings", "IsActived", "Name", "NetIncome", "Password", "PendingClearance", "Type", "UpdatedAt", "UsedForPurchases", "Withdrawn" },
                values: new object[,]
                {
                    { "2da0c54d-afc3-475d-830c-78cac920b4a4", 0L, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7630), "", "test@gmail.com", 0L, true, "Test 1", 0L, "123123123", 0L, 1, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7660), 0L, 0L },
                    { "48eb824c-dbed-4b74-b1f7-9b84a68ae45a", 0L, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7665), "", "admin@admin.com", 0L, true, "admin", 0L, "123123123", 0L, 0, new DateTime(2022, 7, 23, 22, 6, 14, 672, DateTimeKind.Local).AddTicks(7665), 0L, 0L }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Social Media Marketing" },
                    { 2, 1, "Social Media Advertising" },
                    { 3, 1, "Search Engine Optimization (SEO)" },
                    { 4, 1, "Local SEO" },
                    { 5, 1, "Marketing Strategy" },
                    { 6, 1, "Public Relations" },
                    { 7, 1, "Guest Posting" },
                    { 8, 1, "Video Marketing" },
                    { 9, 1, "Email Marketing" },
                    { 10, 1, "Web Analytics" },
                    { 11, 1, "Text Message Marketing" },
                    { 12, 1, "Crowdfunding" },
                    { 13, 1, "Marketing Advice" },
                    { 14, 1, "Search Engine Marketing (SEM)" },
                    { 15, 1, "Display Advertising" },
                    { 16, 1, "E-Commerce Marketing" },
                    { 17, 1, "Influencer Marketing" },
                    { 18, 1, "Community Management" },
                    { 19, 1, "Mobile App Marketing" },
                    { 20, 1, "Music Promotion" },
                    { 21, 1, "Book & eBook Marketing" },
                    { 22, 1, "Podcast Marketing" },
                    { 23, 1, "Affiliate Marketing" },
                    { 24, 1, "Other" },
                    { 25, 2, "Logo Design" },
                    { 26, 2, "Brand Style Guides" },
                    { 27, 2, "Fonts & TypographyNEW" },
                    { 28, 2, "Business Cards & Stationery" },
                    { 29, 2, "Game Art" },
                    { 30, 2, "Graphics for Streamers" },
                    { 31, 2, "Twitch Store" },
                    { 32, 2, "Illustration" },
                    { 33, 2, "NFT Art" },
                    { 34, 2, "Pattern Design" },
                    { 35, 2, "Portraits & Caricatures" },
                    { 36, 2, "Cartoons & Comics" },
                    { 37, 2, "Tattoo Design" },
                    { 38, 2, "Storyboards" },
                    { 39, 2, "Website Design" },
                    { 40, 2, "App Design" },
                    { 41, 2, "UX Design" },
                    { 42, 2, "Landing Page Design" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 43, 2, "Icon Design" },
                    { 44, 2, "Social Media Design" },
                    { 45, 2, "Email Design" },
                    { 46, 2, "Web Banners" },
                    { 47, 2, "Signage Design" },
                    { 48, 2, "Packaging & Covers" },
                    { 49, 2, "Packaging & Label Design" },
                    { 50, 2, "Book Design" },
                    { 51, 2, "Album Cover Design" },
                    { 52, 2, "Podcast Cover Art" },
                    { 53, 2, "Car Wraps" },
                    { 54, 3, "Articles & Blog Posts" },
                    { 55, 3, "Translation" },
                    { 56, 3, "Proofreading & Editing" },
                    { 57, 3, "Resume Writing" },
                    { 58, 3, "Cover Letters" },
                    { 59, 3, "LinkedIn Profiles" },
                    { 60, 3, "Ad Copy" },
                    { 61, 3, "Sales Copy" },
                    { 62, 3, "Social Media Copy" },
                    { 63, 3, "Email Copy" },
                    { 64, 3, "Case Studies" },
                    { 65, 3, "Book & eBook Writing" },
                    { 66, 3, "Book Editing" },
                    { 67, 3, "Scriptwriting" },
                    { 68, 3, "Podcast Writing" },
                    { 69, 3, "Beta Reading" },
                    { 70, 3, "Creative Writing" },
                    { 71, 3, "Brand Voice & Tone" },
                    { 72, 3, "UX Writing" },
                    { 73, 3, "Speechwriting" },
                    { 74, 3, "eLearning Content Development" },
                    { 75, 3, "Technical Writing" },
                    { 76, 4, "Video Editing" },
                    { 77, 4, "Short Video Ads" },
                    { 78, 4, "Whiteboard & Animated Explainers" },
                    { 79, 4, "Character Animation" },
                    { 80, 4, "Lyric & Music Videos" },
                    { 81, 4, "Logo Animation" },
                    { 82, 4, "Intros & Outros" },
                    { 83, 4, "Visual Effects" },
                    { 84, 4, "Subtitles & Captions" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 85, 4, "Spokesperson Videos" },
                    { 86, 4, "Unboxing Videos" },
                    { 87, 4, "Animated GIFs" },
                    { 88, 5, "Voice Over" },
                    { 89, 5, "Producers & Composers" },
                    { 90, 5, "Singers & Vocalists" },
                    { 91, 5, "Mixing & Mastering" },
                    { 92, 5, "Session Musicians" },
                    { 93, 5, "Online Music Lessons" },
                    { 94, 5, "Podcast Editing" },
                    { 95, 5, "Songwriters" },
                    { 96, 5, "Beat Making" },
                    { 97, 5, "Audiobook Production" },
                    { 98, 5, "Audio Ads Production" },
                    { 99, 5, "Sound Design" },
                    { 100, 6, "WordPress" },
                    { 101, 6, "Website Builders & CMS" },
                    { 102, 6, "Game Development" },
                    { 103, 6, "Development for Streamers" },
                    { 104, 6, "Web Programming" },
                    { 105, 6, "E-Commerce Development" },
                    { 106, 6, "Mobile Apps" },
                    { 107, 6, "Desktop Applications" },
                    { 108, 6, "Chatbots" },
                    { 109, 6, "Support & IT" },
                    { 110, 6, "Online Coding Lessons" },
                    { 111, 6, "Cybersecurity & Data Protection" },
                    { 112, 6, "Electronics Engineering" },
                    { 113, 6, "Convert Files" },
                    { 114, 6, "User Testing" },
                    { 115, 6, "QA & Review" },
                    { 116, 6, "Blockchain & Cryptocurrency" },
                    { 117, 6, "NFT Development" },
                    { 118, 6, "Databases" },
                    { 119, 6, "Data Processing" },
                    { 120, 6, "Data Engineering" },
                    { 121, 6, "Data Science" },
                    { 122, 6, "Other" },
                    { 123, 6, "" },
                    { 124, 7, "Virtual Assistant" },
                    { 125, 7, "E-Commerce Management" },
                    { 126, 7, "Market Research" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 127, 7, "Sales" },
                    { 128, 7, "Customer Care" },
                    { 129, 7, "CRM Management NEW" },
                    { 130, 7, "ERP ManagementNEW" },
                    { 131, 7, "Supply Chain Management" },
                    { 132, 7, "Project Management" },
                    { 133, 7, "Event ManagementNEW" },
                    { 134, 7, "Game Concept Design" },
                    { 135, 7, "Business Plans" },
                    { 136, 7, "Financial Consulting" },
                    { 137, 7, "Legal Consulting" },
                    { 138, 7, "Business Consulting" },
                    { 139, 7, "Presentations" },
                    { 140, 7, "HR Consulting" },
                    { 141, 7, "Career Counseling" },
                    { 142, 7, "Data Entry" },
                    { 143, 7, "Data Analytics" },
                    { 144, 7, "Data Visualization" },
                    { 145, 7, "Other" },
                    { 146, 8, "Online Tutoring" },
                    { 147, 8, "Gaming" },
                    { 148, 8, "Astrology & Psychics" },
                    { 149, 8, "Modeling & Acting" },
                    { 150, 8, "Wellness" },
                    { 151, 8, "Traveling" },
                    { 152, 8, "Fitness Lessons" },
                    { 153, 8, "Dance Lessons" },
                    { 154, 8, "Life Coaching" },
                    { 155, 8, "Greeting Cards & Videos" },
                    { 156, 8, "Personal Stylists" },
                    { 157, 8, "Cooking Lessons" },
                    { 158, 8, "Craft Lessons" },
                    { 159, 8, "Arts & Crafts" },
                    { 160, 8, "Family & Genealogy" },
                    { 161, 8, "Collectibles" },
                    { 162, 8, "Other" },
                    { 163, 9, "Dropshipping" },
                    { 164, 9, "E-Commerce Marketing" },
                    { 165, 9, "Game Development" },
                    { 166, 9, "Discord Services" },
                    { 167, 9, "NFT Services" },
                    { 168, 9, "Architecture & Interior Design" }
                });

            migrationBuilder.InsertData(
                table: "Subcategory",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { 169, 9, "Data" },
                    { 170, 9, "Resume Writing" },
                    { 171, 9, "Search Engine Optimization (SEO)" },
                    { 172, 9, "Character Modeling" },
                    { 173, 9, "Character Animation" },
                    { 174, 9, "Image Editing" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Subcategory",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "2da0c54d-afc3-475d-830c-78cac920b4a4");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: "48eb824c-dbed-4b74-b1f7-9b84a68ae45a");

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
