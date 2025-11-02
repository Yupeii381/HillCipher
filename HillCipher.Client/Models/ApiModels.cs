using System;
using System.Collections.Generic;

namespace HillCipher.Client.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class TextItem
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AddTextRequest
    {
        public string Content { get; set; } = string.Empty;
    }

    public class CipherRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Alphabet { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }

    public class CipherResult
    {
        public string Encrypted { get; set; } = string.Empty;
    }
}
