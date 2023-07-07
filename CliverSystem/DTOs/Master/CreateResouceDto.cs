namespace CliverSystem.DTOs.Master
{
    public class CreateResouceDto
    {
        public CreateResouceDto()
        {
            Name = null!;
            Url = null!;
        }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }
    }
}
