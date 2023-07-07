using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table("Notification")]
    public class Notification
    {
        public Notification()
        {
            Description = null!;
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        public NotificationType Type { get; set; }
        public int UserId{ get; set; }
        public User? User{ get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        public DateTime CreatedAt{ get; set; }
    }

}
