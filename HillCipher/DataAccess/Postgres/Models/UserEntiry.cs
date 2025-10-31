using System.ComponentModel.DataAnnotations;

namespace HillCipher.DataAccess.Postgres.Models
{
    public class UserEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public int TokenVersion { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TextEntity> Texts { get; set; } = new List<TextEntity>();
        public ICollection<RequestHistory> RequestHistories { get; set; } = new List<RequestHistory>();
    }


}