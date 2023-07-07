using System.ComponentModel.DataAnnotations;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs.RequestFeatures
{
    public class PostParameters : PaginationOptions
    {
        public string? Search{ get; set; }
        public int? MinPrice{ get; set; }
        public int? MaxPrice { get; set; }
        public int? DeliveryTime { get; set; }
        public PostStatus? Status{ get; set; }
        public PostFilter? Filter { get; set; }
        public int? CategoryId{ get; set; }
        public int? SubCategoryId{ get; set; }
    }
}
