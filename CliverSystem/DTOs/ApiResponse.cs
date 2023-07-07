using CliverSystem.DTOs.RequestFeatures;

namespace CliverSystem.DTOs
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public MetaData? MetaData{ get; set; }
        public ApiResponse(T data, string message, MetaData? metaData = null)
        {
            this.Data = data;
            this.Message = message;
            MetaData = metaData;
        }
    }
}
