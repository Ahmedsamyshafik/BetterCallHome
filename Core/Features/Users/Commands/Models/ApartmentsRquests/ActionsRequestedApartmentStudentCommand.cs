using Core.Bases;
using MediatR;

namespace Core.Features.Users.Commands.Models.ApartmentsRquests
{
    public class ActionsRequestedApartmentStudentCommand :IRequest<Response<string>>
    {
        public int id { get; set; }
        public bool Accept { get; set; }
    }
}
