using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        
        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
