using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace HillCipher.DataAccess.Postgres.Models;

public class RequestHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [MaxLength(50)] public string Action { get; set; } = string.Empty; 
    [MaxLength(200)] public string ActionName { get; set; } = string.Empty; 
    public bool Success { get; set; } = true;            
    [MaxLength(500)] public string? ErrorMessage { get; set; } 

    [MaxLength(500)] public string? InputText { get; set; }
    [MaxLength(500)] public string? ResultText { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}