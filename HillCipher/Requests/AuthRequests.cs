using System.ComponentModel.DataAnnotations;

namespace HillCipher.Requests;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    [Required]
    public string OldPassword { get; set; } = string.Empty;
    [Required]
    public string NewPassword {  get; set; } = string.Empty;
}