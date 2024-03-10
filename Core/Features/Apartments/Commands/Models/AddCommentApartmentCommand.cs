using Core.Bases;
using MediatR;

namespace Core.Features.Apartments.Commands.Models
{
    public class AddCommentApartmentCommand : IRequest<Response<string>>
    {

        public string UserID { get; set; }
        public int ApartmentID { get; set; }
        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public string Comment { get; set; }
    }
}
