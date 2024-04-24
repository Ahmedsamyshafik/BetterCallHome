using Core.Bases;
using Core.Features.Apartments.Queries.Results;
using MediatR;

namespace Core.Features.Apartments.Queries.Models
{
    public class GetApartmentForEditQuery : IRequest<Response<GetApartmentForEditResponse>>
    {
        public int ApartmentId { get; set; } 
    }
}
