using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("Room")]
    public class Room
    {
        public Room()
        {
            //Members = new HashSet<User>();
            MemberKeys = string.Empty;
            //RoomMembers = new List<RoomMember>();
        }

        public int Id { get; set; }
        public string MemberKeys { get; set; }
        public int? LastMessageId { get; set; }
        [NotMapped]
        public Message? LastMessage { get; set; }
        public List<RoomMember>? RoomMembers { get; set; }
        public List<User> Members { get; set; }
        public ICollection<Message>? Messages { get; set; }

        public void SetMemberKeys(params string[] members)
        {
            MemberKeys = string.Join("+", members);
        }
    }
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasMany(r => r.Messages).WithOne(r => r.Room).HasForeignKey(r => r.RoomId);
            builder.HasIndex(r => r.MemberKeys).IsUnique();

            builder
            .HasMany(p => p.Members)
            .WithMany(p => p.Rooms)
            .UsingEntity<RoomMember>(
            r => r.HasOne(rm => rm.Member).WithMany(u => u.RoomMembers).HasForeignKey(rm => rm.MemberId),
            r => r.HasOne(rm => rm.Room).WithMany(u => u.RoomMembers).HasForeignKey(rm => rm.RoomId),
            rm =>
            {
                rm.HasKey(t => new { t.RoomId, t.MemberId});
            }
            );
        }
    }
}
