using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class ChangePasswordUserCommand : IRequest<Response<string>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public ChangePasswordUserCommand(string email)
        {
            Email = email;
        }
    }
}
