using HillCipher.Client.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace HillCipher.Client.Services
{
    public interface IApiService
    {
        Task<ApiResponse<object>?> LoginAsync(string username, string password);
        Task<ApiResponse<object>?> RegisterAsync(string username, string password);
        Task<ApiResponse<IEnumerable<TextItem>>?> GetTextsAsync();
        Task<ApiResponse<TextItem>?> AddTextAsync(string content);
        Task<ApiResponse<CipherResult>?> EncryptTextAsync(int textId, string key, string alphabet);
        Task<ApiResponse<CipherResult>?> DecryptTextAsync(int textId, string key, string alphabet);
        void SetAuthToken(string token);
        bool IsAuthenticated { get; }
    }

    public class ApiService : IApiService
    {
        private readonly RestClient _client;
        private string? _authToken;

        public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);

        public ApiService()
        {
            var options = new RestClientOptions("https://localhost:7099/api/")
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
            };
            _client = new RestClient(options);
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
        }

        public async Task<ApiResponse<object>?> LoginAsync(string username, string password)
        {
            var request = new RestRequest("auth/login", Method.Post);
            request.AddJsonBody(new LoginRequest { Username = username, Password = password });

            var response = await _client.ExecuteAsync<ApiResponse<object>>(request);
            return HandleResponse(response);
        }

        public async Task<ApiResponse<object>?> RegisterAsync(string username, string password)
        {
            var request = new RestRequest("auth/register", Method.Post);
            request.AddJsonBody(new RegisterRequest { Username = username, Password = password });

            var response = await _client.ExecuteAsync<ApiResponse<object>>(request);
            return HandleResponse(response);
        }

        public async Task<ApiResponse<IEnumerable<TextItem>>?> GetTextsAsync()
        {
            var request = new RestRequest("text", Method.Get);
            AddAuthHeader(request);

            var response = await _client.ExecuteAsync<ApiResponse<IEnumerable<TextItem>>>(request);
            return HandleResponse(response);
        }

        public async Task<ApiResponse<TextItem>?> AddTextAsync(string content)
        {
            var request = new RestRequest("text", Method.Post);
            AddAuthHeader(request);
            request.AddJsonBody(new AddTextRequest { Content = content });

            var response = await _client.ExecuteAsync<ApiResponse<TextItem>>(request);
            return HandleResponse(response);
        }

        public async Task<ApiResponse<CipherResult>?> EncryptTextAsync(int textId, string key, string alphabet)
        {
            var request = new RestRequest($"text/{textId}/encrypt", Method.Post);
            AddAuthHeader(request);
            request.AddJsonBody(new CipherRequest { Key = key, Alphabet = alphabet });

            var response = await _client.ExecuteAsync<ApiResponse<CipherResult>>(request);
            return HandleResponse(response);
        }

        public async Task<ApiResponse<CipherResult>?> DecryptTextAsync(int textId, string key, string alphabet)
        {
            var request = new RestRequest($"text/{textId}/decrypt", Method.Post);
            AddAuthHeader(request);
            request.AddJsonBody(new CipherRequest { Key = key, Alphabet = alphabet });

            var response = await _client.ExecuteAsync<ApiResponse<CipherResult>>(request);
            return HandleResponse(response);
        }

        private void AddAuthHeader(RestRequest request)
        {
            if (!string.IsNullOrEmpty(_authToken))
            {
                request.AddHeader("Authorization", $"Bearer {_authToken}");
            }
        }

        private T? HandleResponse<T>(RestResponse<T> response) where T : class
        {
            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            // Handle error
            Console.WriteLine($"API Error: {response.ErrorMessage}");
            return null;
        }
    }
}
