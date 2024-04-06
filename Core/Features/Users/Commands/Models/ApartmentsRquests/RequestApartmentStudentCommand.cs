using Core.Bases;
using MediatR;

namespace Core.Features.Users.Commands.Models.ApartmentsRquests
{
    public class RequestApartmentStudentCommand : IRequest<Response<string>>
    {
        public string UserID { get; set; }

        public int ApartmentID { get; set; }

        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
