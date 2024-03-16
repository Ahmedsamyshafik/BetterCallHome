using Core.Features.Users.Commands.Models;
using Domin.Models;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile
    {
        public void EditProfileCommandToApplicationUser_CommaneMapping()
        {
            //map EditProfileCommand to Application User //EditProfileCommandMapping
            CreateMap<EditProfileUserCommand, ApplicationUser>().
                ForMember(dest=>dest.UserName,opt=>opt.MapFrom(src=>src.Name));
            // password? oldPassword!! Img

        }

       
    }
}
