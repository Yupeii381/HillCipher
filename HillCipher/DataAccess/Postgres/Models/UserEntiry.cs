using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HillCipher.DataAccess.Postgres.Models
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;
        [JsonIgnore]
        public int TokenVersion { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<TextEntity> Texts { get; set; } = new List<TextEntity>();
        [JsonIgnore]
        public ICollection<RequestHistory> RequestHistories { get; set; } = new List<RequestHistory>();
    }


}