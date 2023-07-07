using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var categories = @"Graphics & Design
Digital Marketing
Writing & Translation
Video & Animation
Music & Audio
Programming & Tech
Business
Lifestyle
Trending".Split("\n").Select((item, index) => new Category { Id = index + 1, Name = item.Trim().Replace("\r", "") });
            modelBuilder.Entity<Category>().HasData(categories);

            var subcategories = new[] {
            new
            {
                values = @"Social Media Marketing
                Social Media Advertising
                Search Engine Optimization (SEO)
                Local SEO
                Marketing Strategy
                Public Relations
                Guest Posting
                Video Marketing
                Email Marketing
                Web Analytics
                Text Message Marketing
                Crowdfunding
                Marketing Advice
                Search Engine Marketing (SEM)
                Display Advertising
                E-Commerce Marketing
                Influencer Marketing
                Community Management
                Mobile App Marketing
                Music Promotion
                Book & eBook Marketing
                Podcast Marketing
                Affiliate Marketing
                Other".Split("\n"),
                categoryId = 1
            },
                new
                {
                    values=@"Logo Design
                    Brand Style Guides
                    Fonts & TypographyNEW
                    Business Cards & Stationery
                    Game Art
                    Graphics for Streamers
                    Twitch Store
                    Illustration
                    NFT Art
                    Pattern Design
                    Portraits & Caricatures
                    Cartoons & Comics
                    Tattoo Design
                    Storyboards
                    Website Design
                    App Design
                    UX Design
                    Landing Page Design
                    Icon Design
                    Social Media Design
                    Email Design
                    Web Banners
                    Signage Design
                    Packaging & Covers
                    Packaging & Label Design
                    Book Design
                    Album Cover Design
                    Podcast Cover Art
                    Car Wraps".Split("\n"),
                    categoryId = 2
                },
                new
                {
                    values=@"Articles & Blog Posts
            Translation
            Proofreading & Editing
            Resume Writing
            Cover Letters
            LinkedIn Profiles
            Ad Copy
            Sales Copy
            Social Media Copy
            Email Copy
            Case Studies
            Book & eBook Writing
            Book Editing
            Scriptwriting
            Podcast Writing
            Beta Reading
            Creative Writing
            Brand Voice & Tone
            UX Writing
            Speechwriting
            eLearning Content Development
            Technical Writing".Split("\n"),
                    categoryId = 3
                },
                new
                {
                    values=@"Video Editing
Short Video Ads
Whiteboard & Animated Explainers
Character Animation
Lyric & Music Videos
Logo Animation
Intros & Outros
Visual Effects
Subtitles & Captions
Spokesperson Videos
Unboxing Videos
Animated GIFs".Split("\n"),
                    categoryId = 4
                },
                new
                {
                    values=@"Voice Over
Producers & Composers
Singers & Vocalists
Mixing & Mastering
Session Musicians
Online Music Lessons
Podcast Editing
Songwriters
Beat Making
Audiobook Production
Audio Ads Production
Sound Design".Split("\n"),
                    categoryId = 5
                },
                new
                {
                    values=@"WordPress
                            Website Builders & CMS
                            Game Development
                            Development for Streamers
                            Web Programming
                            E-Commerce Development
                            Mobile Apps
                            Desktop Applications
                            Chatbots
                            Support & IT
                            Online Coding Lessons
                            Cybersecurity & Data Protection
                            Electronics Engineering
                            Convert Files
                            User Testing
                            QA & Review
                            Blockchain & Cryptocurrency
                            NFT Development
                            Databases
                            Data Processing
                            Data Engineering
                            Data Science
                            Other
                        ".Split("\n"),
                        categoryId = 6
                },
                new
                {
                    values=@"Virtual Assistant
                            E-Commerce Management
                            Market Research
                            Sales
                            Customer Care
                            CRM Management NEW
                            ERP ManagementNEW
                            Supply Chain Management
                            Project Management
                            Event ManagementNEW
                            Game Concept Design
                            Business Plans
                            Financial Consulting
                            Legal Consulting
                            Business Consulting
                            Presentations
                            HR Consulting
                            Career Counseling
                            Data Entry
                            Data Analytics
                            Data Visualization
                            Other".Split("\n"),
                    categoryId = 7
                },
                new
                {
                    values=@"Online Tutoring
Gaming
Astrology & Psychics
Modeling & Acting
Wellness
Traveling
Fitness Lessons
Dance Lessons
Life Coaching
Greeting Cards & Videos
Personal Stylists
Cooking Lessons
Craft Lessons
Arts & Crafts
Family & Genealogy
Collectibles
Other".Split("\n"),
                    categoryId = 8
                },
                new
                {
                    values=@"Dropshipping
E-Commerce Marketing
Game Development
Discord Services
NFT Services
Architecture & Interior Design
Data
Resume Writing
Search Engine Optimization (SEO)
Character Modeling
Character Animation
Image Editing".Split("\n"),
                    categoryId = 9
                }
            };

            var listSubcategories = new List<Subcategory>();
            foreach (var item in subcategories)
            {
                var kq = item.values.Select((v, index) => new Subcategory { Id = index + 1 + listSubcategories.Count(), Name = v.Replace("\r", "").Trim(), CategoryId = item.categoryId });
                modelBuilder.Entity<Subcategory>().HasData(kq);
                listSubcategories = listSubcategories.Concat(kq).ToList();

            }
            modelBuilder.Entity<Wallet>().HasData(
                    new Wallet()
                    {
                        Id = 1
                    },
                    new Wallet()
                    {
                        Id = 2
                    },
                    new Wallet()
                    {
                        Id = 3
                    },
                    new Wallet()
                    {
                        Id = 4
                    },
                    new Wallet()
                    {
                        Id = 9
                    },
                    new Wallet()
                    {
                        Id = 10
                    }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "53f891d8-bd32-40cf-a30c-04f2d5ecf164",
                    Name = "Test 1",
                    Email = "test@gmail.com",
                    Password = "123123123",
                    Avatar = "https://picsum.photos/200",
                    IsVerified = true,
                    WalletId = 1,
                    CreatedAt = new DateTime(2022, 7, 25),
                    UpdatedAt = new DateTime(2022, 7, 25)
                },
                new User
                {
                    Id = "3dbb7902-74a5-4113-9052-a13919a73949",
                    Name = "Test 2",
                    Email = "test2@gmail.com",
                    Password = "123123123",
                    Avatar = "https://picsum.photos/200",
                    IsVerified = true,
                    WalletId = 2,
                    CreatedAt = new DateTime(2022, 7, 25),
                    UpdatedAt = new DateTime(2022, 7, 25),
                },
                new User
                {
                    Id = "f5a6e9d2-a322-4e0d-bf39-8acf7b6b2fc6",
                    Name = "Test 3",
                    Email = "test3@gmail.com",
                    Avatar = "https://picsum.photos/200",
                    Password = "123123123",
                    IsVerified = true,
                    WalletId = 3,
                    CreatedAt = new DateTime(2022, 7, 25),
                    UpdatedAt = new DateTime(2022, 7, 25),
                },
                new User
                {
                    Id = "fedb88e2-decb-45a2-a0f1-8edc92b0b918",
                    Name = "admin",
                    Avatar = "https://picsum.photos/200",
                    Email = "admin@admin.com",
                    Password = "123123123",
                    Type = Common.Enum.UserType.Admin,
                    IsVerified = true,
                    WalletId = 4,
                    CreatedAt = new DateTime(2022, 7, 25),
                    UpdatedAt = new DateTime(2022, 7, 25),
                },
                new User
                {
                    Id = "733a44cc-f3b5-4e79-8dda-afb6be9c72a3",
                    Name = "test 4",
                    Avatar = "https://picsum.photos/200",
                    Email = "test4@gmail.com",
                    Password = "123123123",
                    Type = Common.Enum.UserType.User,
                    IsVerified = true,
                    WalletId = 9,
                    CreatedAt = new DateTime(2022, 12, 25),
                    UpdatedAt = new DateTime(2022, 12, 25),
                },
                new User
                {
                    Id = "19328465-fcf8-4315-b687-bba6b86d13ed",
                    Name = "test 5",
                    Avatar = "https://picsum.photos/200",
                    Email = "test5@gmail.com",
                    Password = "123123123",
                    Type = Common.Enum.UserType.User,
                    IsVerified = true,
                    WalletId = 10,
                    CreatedAt = new DateTime(2022, 12, 25),
                    UpdatedAt = new DateTime(2022, 12, 25),
                }
            );

            modelBuilder.Entity<Parameters>().HasData(
            new Parameters { NumDaysReturnMoney = 5 }
            );
        }

    }
}
