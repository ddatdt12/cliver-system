using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    public class SavedService
    {
        public SavedService()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public int SavedListId { get; set; }
        public SavedList? SavedList { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SavedServiceConfiguration : IEntityTypeConfiguration<SavedService>
    {
        public void Configure(EntityTypeBuilder<SavedService> builder)
        {
            builder.HasKey(x => new { x.SavedListId, x.PostId });
        }
    }
}
