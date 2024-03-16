using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class ForgetPasswordUserCommand : IRequest<Response<string>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ForgetPasswordUserCommand(string email)
        {
            Email = email;
        }
    }
}
