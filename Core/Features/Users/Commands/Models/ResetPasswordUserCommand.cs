using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class ResetPasswordUserCommand : IRequest<Response<string>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(255)]
        [Required]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(255)]
        [Compare("NewPassword")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
