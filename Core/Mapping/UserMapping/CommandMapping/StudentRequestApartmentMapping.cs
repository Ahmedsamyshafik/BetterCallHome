using Core.Features.Users.Commands.Models.ApartmentsRquests;
using Domin.Models;
using Infrastructure.DTO;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile
    {
        
        public void RequestApartmentStudentCommandToUserApartment()
        {
            // mapping from student_Request To UserApartment
            CreateMap<RequestApartmentStudentCommand, UserApartmentsRequests>();
        }
    }
}
