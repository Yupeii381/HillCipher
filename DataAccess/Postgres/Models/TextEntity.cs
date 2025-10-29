using System.ComponentModel.DataAnnotations;

namespace HillCipher.DataAccess.Postgres.Models
{
    public class TextEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Plaintext { get; set; } = string.Empty;

        [Required]
        public string Ciphertext { get; set; } = string.Empty;

        [Required]
        public string Alphabet { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
