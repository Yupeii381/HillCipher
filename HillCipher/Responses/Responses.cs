using System.Globalization;

namespace HillCipher.Responses;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Success(T data, string? message = null)
        => new() { Data = data, Message = message, IsSuccess = true };

    public static ApiResponse<T> Success(string? message = "Операция выполена успешно.")
        => new() { Message = message, IsSuccess = true };

    public static ApiResponse<T> Failure(string? message = "Произошла ошибка", List<string>? errors = null)
        => new() { Message = message, Errors = errors ?? new(), IsSuccess = false };
}

public record AuthResponse(
    string Token,
    string Username
);