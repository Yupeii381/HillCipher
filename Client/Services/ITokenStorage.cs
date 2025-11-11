namespace Client.Services;

public interface ITokenStorage
{
    string? GetToken();
    void SetToken(string token);
    void Clear();
}