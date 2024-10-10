namespace CostumerSolution.API.Application.Response
{
    public class BaseResponse<T>(bool success, string message, int statusCode, T? data = default)
    {
        public T? Data { get; } = data;
        public bool Success { get; } = success;
        public string Message { get; } = message;
        public int StatusCode { get; } = statusCode;
    }

}
