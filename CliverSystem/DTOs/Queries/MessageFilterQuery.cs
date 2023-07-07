namespace CliverSystem.DTOs.Queries
{
    public class MessageFilterQuery
    {
        public MessageFilterQuery()
        {
            Offset = 0;
            Limit = 20;
        }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
