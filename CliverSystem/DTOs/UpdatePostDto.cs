using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
    public class UpdatePostDto
    {
        public UpdatePostDto()
        {
        }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? SubcategoryId { get; set; }

        private List<string>? _tags { get; set; }
        public List<string>? Tags
        {
            get { return _tags; }
            set
            {
                _tags = value?.Select(s => s.Trim().Replace(";", "")).ToList();
            }
        }
        private List<string>? _images { get; set; }
        public List<string>? Images
        {
            get { return _images; }
            set
            {
                _images = value?.Select(s => s.Trim().Replace(";", "")).ToList();
            }
        }
        public string? Document { get; set; }
        public bool? HasOfferPackages { get; set; }
        public bool? IsPublish{ get; set; }
        public ICollection<UpsertPackageDto>? Packages { get; set; }
    }
}
