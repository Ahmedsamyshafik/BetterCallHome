using Infrastructure.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTO
{
    public class RegisterDTO
    {

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [PhoneValidation]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [AgeValidation]
        public int Age { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string College { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string University { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [MinLength(6)]
        [MaxLength(255)]
        public string COPassword { get; set; }

    }
}
