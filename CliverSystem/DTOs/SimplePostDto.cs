using CliverSystem.Models;
using System.ComponentModel;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class SimplePostDto : AuditEntity
    {
        public SimplePostDto()
        {
        }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserId { get; set; } = null!;
        public PostStatus Status { get; set; }
        public UserDto? User { get; set; }
        public int SubcategoryId { get; set; }
        public Subcategory? Subcategory { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Images { get; set; } = new List<string>();
        public double RatingAvg { get; set; }
        public int RatingCount { get; set; }
    }

}
