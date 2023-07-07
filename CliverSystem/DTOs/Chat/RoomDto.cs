namespace CliverSystem.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int? LastMessageId { get; set; }
        public MessageDto? LastMessage { get; set; } = null; 
        public List<UserDto> Members { get; set; } = new List<UserDto>();
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
    }
}
