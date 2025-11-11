namespace Client.Services;

public record ApiResponse<T>(
    bool IsSuccess,
    T? Data,
    string? ErrorMessage = null
);
