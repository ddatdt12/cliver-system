namespace CliverSystem.DTOs
{
    public class CreateMessageDto
    {
        public string Content { get; set; } = string.Empty;
        public string SenderId { get; set; } = string.Empty;
        public string? ReceiverId { get; set; }
        public int? RoomId { get; set; }
        public int? PostId { get; set; }
        public int? RepliedToMessageId { get; set; }
        public int? CustomPackageId { get; set; }
    }
}
