using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table("ServiceRequest")]
    public class ServiceRequest : AuditEntity
    {
        public ServiceRequest()
        {
            Description = string.Empty;
            Description = string.Empty;
            Tags = string.Empty;
            UserId = null!;
        }
        public int Id { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "varchar(36)")]
        public string UserId { get; set; }
        public User? User { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int? SubcategoryId { get; set; }
        public Subcategory? Subcategory { get; set; }
        public string Tags { get; set; }
        public long? Budget { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? DoneAt { get; set; }
    }

    public class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
    {
        public void Configure(EntityTypeBuilder<ServiceRequest> builder)
        {
        }
    }
}
