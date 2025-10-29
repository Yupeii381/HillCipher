using System.ComponentModel.DataAnnotations;

namespace HillCipher.Requests;

public class CipherRequest
{
    [Required]
    public string Key { get; set; } = string.Empty;
    [Required]
    public string Text { get; set; } = string.Empty;
    [Required]
    public string Alphabet { get; set; } = string.Empty;
}
