using Core.Features.Users.Commands.Models;
using Infrastructure.DTO;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile
    {
        public void ResetPasswordUserCommandToResetPasswordDto_CommandMapping()
        {
            //mapping from ResetPasswordUserCommand To ResetPasswordDTO
            CreateMap<ResetPasswordUserCommand, ResetPasswordDTO>();
        }


    }
}
