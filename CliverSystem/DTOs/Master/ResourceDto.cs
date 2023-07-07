namespace CliverSystem.DTOs.Master
{
    public class ResourceDto
    {
        public ResourceDto()
        {
            Name = null!;
            Url = null!;
        }
        public int  Id{ get; set; }
        public string Name{ get; set; }
        public int Size{ get; set; }
        public string Url { get; set; }
    }
}
