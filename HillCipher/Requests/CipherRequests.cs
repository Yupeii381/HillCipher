namespace HillCipher.Requests;

public class AddTextRequest
{
    public string Content { get; set; } = null!;
}

public class UpdateTextRequest
{
    public string Content { get; set; } = null!;
}

public class CipherRequest
{
    public string Key { get; set; } = null!;
    public string Alphabet { get; set; } = null!;
}