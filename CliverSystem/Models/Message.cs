using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("Message")]
    public class Message : AuditEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column(TypeName = "varchar(36)")]
        public string SenderId { get; set; } = string.Empty;
        public User? Sender { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public int? PostId { get; set; }
        public Post? RelatedPost { get; set; }
        public int? RepliedToMessageId { get; set; }
        public Message? RepliedToMessage { get; set; }
        public int? CustomPackageId { get; set; }
        [ForeignKey("CustomPackageId")]
        public Package? CustomPackage { get; set; }
    }
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(r => r.Room).WithMany(r => r.Messages).HasForeignKey(r => r.RoomId);
        }
    }
}

