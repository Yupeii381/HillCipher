using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HillCipher.DataAccess.Postgres.Models
{
    public class RequestHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public UserEntity User { get; set; } = null!;

        public string Action { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? InputText { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ResultText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}