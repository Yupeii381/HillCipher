using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HillCipher.Client.Services;

public interface IApiService
{
    Task<T> GetAsync<T>(string endpoint);
    Task<T> PostAsync<T>(string endpoint, object data);
    Task<T> PatchAsync<T>(string endpoint, object data);
    Task DeleteAsync(string endpoint);
    void SetAuthToken(string token);
    void ClearAuth();
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public ApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7099/api/") };
    }

    public void SetAuthToken(string token)
    {
        _token = token;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuth()
    {
        _token = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json) ?? throw new Exception("Deserialization failed");
    }

    public async Task<T> PostAsync<T>(string endpoint, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var respJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(respJson) ?? throw new Exception("Deserialization failed");
    }

    public async Task<T> PatchAsync<T>(string endpoint, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint) { Content = content };
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var respJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(respJson) ?? throw new Exception("Deserialization failed");
    }

    public async Task DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }
}