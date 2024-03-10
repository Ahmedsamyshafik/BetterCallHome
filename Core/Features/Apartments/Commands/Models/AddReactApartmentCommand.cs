using Core.Bases;
using MediatR;

namespace Core.Features.Apartments.Commands.Models
{
    public class AddReactApartmentCommand : IRequest<Response<string>>
    {


        public string UserID { get; set; }

        public int ApartmentID { get; set; }

        public DateTime? Date { get; set; } = DateTime.UtcNow;




    }
}
