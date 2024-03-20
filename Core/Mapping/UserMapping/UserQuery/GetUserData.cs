using Core.Features.Users.Queries.Results;
using Domin.Models;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile
    {
        public void ApplicationUserToUserResponse()
        {
            // applicationUser => UserDataResponse 
            // UserName Phone Age  College  University  Gender  Message  IsAuthenticated   Roles   

            CreateMap<ApplicationUser, UserDataResponse>().
                ForMember(dest => dest.imagePath, opt => opt.MapFrom(src => src.imageUrl)).
                ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.PhoneNumber)).
                ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id));
        }
    }
}

