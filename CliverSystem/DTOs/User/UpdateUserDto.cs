using CliverSystem.DTOs.User;
using System.ComponentModel.DataAnnotations;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class UpdateUserDto
    {
        public UpdateUserDto()
        {
        }

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public List<Language>? Languages{ get; set; }
        public List<string>? Skills{ get; set; }
    }
}
