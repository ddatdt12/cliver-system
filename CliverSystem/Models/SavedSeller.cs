using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    public class SavedSeller
    {
        public SavedSeller()
        {
            CreatedAt = DateTime.UtcNow;
        }
        [Column(TypeName = "varchar(36)")]
        public string UserId { get; set; } = null!;
        public User? User { get; set; }
        public int SavedListId { get; set; }
        public SavedList? SavedList { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SavedSellerConfiguration : IEntityTypeConfiguration<SavedSeller>
    {
        public void Configure(EntityTypeBuilder<SavedSeller> builder)
        {
            builder.HasKey(x => new { x.SavedListId, x.UserId });
        }
    }
}
