using CliverSystem.Attributes;
using CliverSystem.DTOs.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class CreateReviewDto
    {
        public CreateReviewDto()
        {
            Comment = String.Empty;
        }
        [Required]
        public int OrderId { get; set; }
        [Required]
        [IntRange(min:1, max:5, ErrorMessage ="Rating must be between 1 and 5")]
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int? Label { get; set; }
    }
}
