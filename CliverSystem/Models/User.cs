using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table("User")]
    public class User : AuditEntity
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            Posts = new List<Post>();
            Rooms = new List<Room>();
            Orders = new List<Order>();
            Rooms = new List<Room>();
            Type = UserType.User;
            IsActive = false;
            IsVerified = false;
            Languages = "";
            Skills = "";
            Avatar = null!;
        }
        [Key]
        [Column(TypeName ="varchar(36)")]
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? IdentityCardImage { get; set; }
        public string Avatar{ get; set; }
        public int WalletId { get; set; }
        public Wallet? Wallet { get;  set; }
        public UserType Type { get; set; }
        public bool IsActive { get; set; }
        public double RatingAvg { get; set; }
        public int RatingCount { get; set; }
        public string Languages { get; set; }
        public string Skills { get; set; }
        public bool IsVerified { get; set; }
        [NotMapped]
        public bool IsSaved { get; set; }
        //public ICollection<RecentPost>? RecentPosts { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Order> Orders{ get; set; }
        public ICollection<Room> Rooms{ get; set; }
        public ICollection<RoomMember>? RoomMembers { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Avatar).HasDefaultValue("https://t4.ftcdn.net/jpg/02/23/50/73/360_F_223507349_F5RFU3kL6eMt5LijOaMbWLeHUTv165CB.jpg");
        }
    }
}
