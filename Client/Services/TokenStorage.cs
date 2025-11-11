using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.IO;
using System.Text.Json;

namespace HillCipher.Client.Services;

public interface ITokenStorage
{
    string? GetToken();
    void SetToken(string token);
    void Clear();
}

public class TokenStorage : ITokenStorage
{
    private readonly string _filePath;

    public TokenStorage()
    {
        // Путь: %AppData%\HillCipher\auth.json (Windows), ~/.local/share/HillCipher/auth.json (Linux/macOS)
        var appData = AvaloniaLocator.Current.GetService<Avalonia.ApplicationLifetime>() switch
        {
            IClassicDesktopStyleApplicationLifetime desktop =>
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HillCipher"),
            _ =>
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HillCipher")
        };

        Directory.CreateDirectory(appData);
        _filePath = Path.Combine(appData, "auth.json");
    }

    public string? GetToken()
    {
        if (!File.Exists(_filePath)) return null;
        try
        {
            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<AuthData>(json);
            return data?.Token;
        }
        catch
        {
            return null;
        }
    }

    public void SetToken(string token)
    {
        var data = new AuthData { Token = token };
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void Clear()
    {
        if (File.Exists(_filePath))
            File.Delete(_filePath);
    }

    private record AuthData(string? Token = null);
}