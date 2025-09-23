using System.ComponentModel.DataAnnotations;

namespace HillCipher.DataAccess.Postgres.Models
{
    public class TextEntity
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string PlainText { get; set; } = string.Empty;
        [Required]
        public string CipherText { get; set; } = string.Empty;
        [Required]
        public string Key { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public UserEntity? User { get; set; }
    }
}
