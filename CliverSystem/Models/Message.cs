namespace CliverSystem.Models
{
    public class Message : AuditEntity
    {
        public int Id { get; set; }
        public string Content{ get; set; } = string.Empty;
        public int SenderId{ get; set; }
        public User? Sender { get; set; }
        public int RoomId{ get; set; }
        public Room? Room { get; set; }

        public int? PostId { get; set; }
        public Post? RelatedPost { get; set; }
    }
}
