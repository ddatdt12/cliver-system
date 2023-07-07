using CliverSystem.Models;

namespace CliverSystem.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public UserDto? Sender { get; set; }
        public int RoomId { get; set; }
        public RoomDto? Room { get; set; }

        public int? PostId { get; set; }
        public PostDto? RelatedPost { get; set; }
        public int? RepliedToMessageId { get; set; }
        public MessageDto? RepliedToMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? CustomPackageId { get; set; }
        public CustomPackageDto? CustomPackage { get; set; }
    }
}
