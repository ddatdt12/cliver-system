using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("RoomMembers")]
    public class RoomMember
    {
        [Column("MemberId", TypeName = "varchar(36)")]
        public string MemberId { get; set; } = string.Empty;
        public User? Member { get; set; }
        [Column("RoomId")]
        public int RoomId { get; set; }
        public Room? Room { get; set; }
    }

    public class RoomMemberConfiguration : IEntityTypeConfiguration<RoomMember>
    {
        public void Configure(EntityTypeBuilder<RoomMember> builder)
        {
            //builder.HasKey(x => new { x.RoomId, x.MemberId });
            //builder.HasOne(x => x.Room).WithMany(x => x.RoomMembers).HasForeignKey(x => x.RoomId);
            //builder.HasOne(x => x.Member).WithMany(x => x.RoomMembers).HasForeignKey(x => x.MemberId);
        }
    }
}
