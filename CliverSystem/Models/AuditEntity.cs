namespace CliverSystem.Models
{
    public abstract class AuditEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
