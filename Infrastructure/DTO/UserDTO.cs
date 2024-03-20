using Infrastructure.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO
{
    public class UserDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string UserName { get; set; }

        public string UserId { get; set; }

        [Required]
        [StringLength(11)]
        [PhoneValidation]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string? College { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string? University { get; set; }

        [Required]
        public string Gender { get; set; }

        public string Message { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }

        public DateTime Expier { get; set; }
    }
}
