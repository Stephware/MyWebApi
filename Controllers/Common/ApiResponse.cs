namespace MyWebApi.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }

    public List<string> Errors { get; set; } = [];
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string? TraceId { get; set; } = null;

    public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful", string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            TraceId = traceId
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, List<string> errors, string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            TraceId = traceId
        };
    }
}