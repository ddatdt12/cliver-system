using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CliverSystem.Models
{
    public class Room
    {
        public Room()
        {
            //Members = new HashSet<User>();
        }

        public int Id { get; set; }
        public Message? LastMessage { get; set; }
        public ICollection<User>? Members { get; set; }
        public ICollection<Message>? Messages{ get; set; }
    }
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasMany(r => r.Messages).WithOne(r => r.Room).HasForeignKey(r => r.RoomId);
            builder.HasOne(r => r.LastMessage).WithOne(r => r.Room).HasForeignKey<Message>(m => m.RoomId);
        }
    }
}
