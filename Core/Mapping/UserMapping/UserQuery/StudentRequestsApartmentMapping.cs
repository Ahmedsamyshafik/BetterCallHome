using Domin.Models;
using Infrastructure.DTO.Student;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile
    {
        public void UserRequestsToApartmentRequestsResponse()
        {
            //_mapper.Map<List<ApartmentRequestsResponse>>(records);
            CreateMap<UserApartmentsRequests, ApartmentRequestsResponse>().
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).
                ForMember(dest => dest.ApartmentId, opt => opt.MapFrom(src => src.Apartment.Id)).
                ForMember(dest => dest.ApartmentName, opt => opt.MapFrom(src => src.Apartment.Name)).
                ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.ApplicationUser.Id)).
                ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.ApplicationUser.UserName)).
                ForMember(dest => dest.StudentImageURL, opt => opt.MapFrom(src => src.ApplicationUser.imageUrl)).
                ForMember(dest => dest.StudentPhone, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber));
        }
    }
}