using System.ComponentModel.DataAnnotations.Schema;

namespace CliverSystem.Models
{
    [Table("Resource")]
    public class Resource
    {
        public Resource()
        {
            Name = null!;
            Url = null!;
        }
        public int  Id{ get; set; }
        public string Name{ get; set; }
        public int Size{ get; set; }
        public string Url{ get; set; }
    }
}
