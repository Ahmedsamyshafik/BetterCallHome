using Core.Features.Users.Commands.Models.ApartmentsRquests;
using Core.Features.Users.Commands.Models.PaymentRequest;
using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.DTO.Payment;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile
    {
        
        public void RequestApartmentStudentCommandToUserApartment()
        {
            // mapping from student_Request To UserApartment
            CreateMap<RequestApartmentStudentCommand, UserApartmentsRequests>();
        }
        public void PaymentMapping()
        {
            CreateMap<FormPaymentCommand, FormViewModel>(); 
        }
    }
}
