using System.ComponentModel.DataAnnotations;

namespace CliverSystem.DTOs
{
    public class CreatePostDto
    {
        public CreatePostDto()
        {
            _Tags = new List<string>();
        }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Category is required")]
        public int SubcategoryId { get; set; }
        private List<string> _Tags { get; set; }
        public List<string> Tags
        {
            get { return _Tags; }
            set
            {
                _Tags = value.Select(s => s.Trim().Replace(";", "")).ToList();
            }
        }
    }
}
