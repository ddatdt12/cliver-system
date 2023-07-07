using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class CreateServiceRequestDto
    {
        public CreateServiceRequestDto()
        {
            Description = string.Empty;
            Tags = new List<string>();
        }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public List<string> Tags { get; set; }
        public long? Budget { get; set; }
        public DateTime? Deadline { get; set; }
    }

}
