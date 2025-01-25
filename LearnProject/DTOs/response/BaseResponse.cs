namespace LearnProject.Dtos.response
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        public BaseResponse()
        {
        }
    }
}
