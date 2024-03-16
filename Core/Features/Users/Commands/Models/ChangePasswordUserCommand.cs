using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class ChangePasswordUserCommand :IRequest<Response<string>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MinLength(6)]
        [MaxLength(255)]
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [MinLength(6)]
        [MaxLength(255)]
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}
