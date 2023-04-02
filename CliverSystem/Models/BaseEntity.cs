namespace CliverSystem.Models
{
    public abstract class BaseEntity<T> where T : class
    {
        public T? Id { get; set; }
    }
}
