using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class ConfirmForgetPasswordUserCommand : IRequest<Response<string>>
    {
        [Required]
        public string code { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }


    }
}
