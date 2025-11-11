using Client.Models;
using Client.Services;
using Client.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public void SetAuthToken(string? token)
    {
        _token = token;
        if (string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = null;
        else
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    public string? GetAuthToken() => _token;

    private void EnsureAuth()
    {
        if (string.IsNullOrEmpty(_token))
            throw new InvalidOperationException("Not authenticated");
    }

    public async Task<string> LoginAsync(LoginRequest loginRequest)
    {
        var res = await _httpClient.PostAsJsonAsync("/api/Auth/Login", new { login, password });
        return await HandleAuth(res);
    }

    public async Task<string> RegisterAsync(RegisterRequest registerRequest)
    {
        var res = await _httpClient.PostAsJsonAsync("/api/Auth/Register", new { registerRequest });
        return await HandleAuth(res);
    }

    private async Task<string> HandleAuth(HttpResponseMessage res)
    {
        if (res.IsSuccessStatusCode)
        {
            var dto = await res.Content.ReadFromJsonAsync<AuthResponse>();
            return dto?.Token ?? throw new InvalidOperationException("No token");
        }
        var err = await res.Content.ReadAsStringAsync();
        throw new ApiException(res.StatusCode, err);
    }

    public async Task<int> AddTextAsync(AddTextRequest addTextRequest)
    {
        EnsureAuth();
        var res = await _httpClient.PostAsJsonAsync("/api/Texts", new { addTextRequest });
        if (res.IsSuccessStatusCode)
        {
            var dto = await res.Content.ReadFromJsonAsync<TextIdResponse>();
            return dto?.Id ?? -1;
        }
        var err = await res.Content.ReadAsStringAsync();
        throw new ApiException(res.StatusCode, err);
    }

    public async Task<string> CryptAsync(int textId, string key, bool encrypt)
    {
        EnsureAuth();
        var mode = encrypt ? "encrypt" : "decrypt";
        var res = await _httpClient.PostAsJsonAsync($"/api/{textId}/crypt", new { key, mode });
        if (res.IsSuccessStatusCode)
        {
            var dto = await res.Content.ReadFromJsonAsync<CryptResponse>();
            return dto?.Result ?? "";
        }
        var err = await res.Content.ReadAsStringAsync();
        throw new ApiException(res.StatusCode, err);
    }

    public async Task<HistoryItem[]> GetHistoryAsync()
    {
        EnsureAuth();
        var res = await _httpClient.GetAsync("/api/history");
        if (res.IsSuccessStatusCode)
        {
            return (await res.Content.ReadFromJsonAsync<HistoryItem[]>()) ?? [];
        }
        throw new ApiException(res.StatusCode, await res.Content.ReadAsStringAsync());
    }
}

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? ResponseBody { get; }

    public ApiException(HttpStatusCode code, string? body)
        : base($"API [{code}]: {body}")
    {
        StatusCode = code;
        ResponseBody = body;
    }
}