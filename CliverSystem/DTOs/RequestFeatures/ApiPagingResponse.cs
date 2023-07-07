namespace CliverSystem.DTOs.RequestFeatures
{
    public class ApiPagingResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public MetaData? MetaData{ get; set; }
        public ApiPagingResponse(T data, string message, MetaData? metaData)
        {
            Data = data;
            Message = message;
            MetaData = metaData;
        }
    }
}
