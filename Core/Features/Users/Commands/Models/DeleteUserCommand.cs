using Core.Bases;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class DeleteUserCommand : IRequest<Response<string>>
    {
        [Required]
        public string UserId { get; set; }
    }
}
