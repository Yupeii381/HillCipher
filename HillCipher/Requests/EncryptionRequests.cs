using System.ComponentModel.DataAnnotations;

namespace HillCipher.Requests;

public class EncryptionRequests
{
    [Required]
    public string Key { get; set; } = string.Empty;
    [Required]
    public string Plaintext { get; set; } = string.Empty;
    [Required]
    public string Alphabet { get; set; } = string.Empty;
}
