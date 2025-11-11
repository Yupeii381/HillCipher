
namespace Client.Models;

public record ChangePasswordRequest(
    string OldPassword,
    string NewPassword
);
