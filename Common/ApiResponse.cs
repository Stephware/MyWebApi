using System.Text.Json.Serialization;

namespace MyWebApi.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public required string Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(
        T data,
        string message = "Request successful")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(
        string message,
        List<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}
