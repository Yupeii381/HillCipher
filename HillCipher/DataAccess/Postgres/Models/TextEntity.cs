using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HillCipher.DataAccess.Postgres.Models
{
    public class TextEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public UserEntity User { get; set; } = null!;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}