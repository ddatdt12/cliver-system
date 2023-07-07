using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("Subcategory")]
    public class Subcategory
    {
        public Subcategory()
        {
            Name = string.Empty;
            Icon = "https://picsum.photos/50";
            Posts = new List<Post>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Icon { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<Post> Posts { get; set; }
    }
}
