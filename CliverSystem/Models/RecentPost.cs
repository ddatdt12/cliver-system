using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("RecentPost")]
    public class RecentPost
    {
        public RecentPost()
        {
            CreatedAt = DateTime.UtcNow;
        }
        [Column(TypeName = "varchar(36)")]
        public string UserId { get; set; } = null!;
        public User? User { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RecentPostConfiguration : IEntityTypeConfiguration<RecentPost>
    {
        public void Configure(EntityTypeBuilder<RecentPost> builder)
        {
            builder.HasKey(x => new { x.UserId, x.PostId });
            //builder.HasOne(x => x.Room).WithMany(x => x.RoomMembers).HasForeignKey(x => x.RoomId);
            //builder.HasOne(x => x.Member).WithMany(x => x.RoomMembers).HasForeignKey(x => x.MemberId);
        }
    }
}
