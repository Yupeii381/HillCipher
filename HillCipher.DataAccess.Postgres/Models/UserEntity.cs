using System.ComponentModel.DataAnnotations;

namespace HillCipher.DataAccess.Postgres.Models
{
    public class UserEntity
    {
        [Required]
        public Guid Id { get; set; }    
        public string Username { get; set; } = string.Empty;    
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public List<TextEntity> Texts { get; set; } = [];
    }
}
