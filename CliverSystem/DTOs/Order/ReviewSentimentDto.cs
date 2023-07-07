using System;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs.Order
{
	public class ReviewSentimentDto
	{
        public ReviewSentimentDto()
        {
            Comment = string.Empty;
        }
        public UserDto? User { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Label { get; set; }
    }
}

