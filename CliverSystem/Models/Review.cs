using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Models
{
    [Table(name: "Review")]
    public class Review
    {
        public Review()
        {
            CreatedAt = DateTime.UtcNow;
            UserId = null!;
            Comment = "";
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Comment{ get; set; }
        public Order? Order { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        public ReviewType Type { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Label { get;set; }

    }
}
