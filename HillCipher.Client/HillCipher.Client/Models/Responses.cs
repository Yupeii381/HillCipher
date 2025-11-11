namespace HillCipher.Client.Models;

public class AuthResponse
{
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

public class EncryptedTextResponse
{
    public string Encrypted { get; set; } = string.Empty;
    public string Decrypted { get; set; } = string.Empty;
}