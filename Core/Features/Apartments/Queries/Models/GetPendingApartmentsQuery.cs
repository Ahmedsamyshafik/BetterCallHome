using Core.Bases;
using Core.Features.Apartments.Queries.Results;
using MediatR;

namespace Core.Features.Apartments.Queries.Models
{
    public class GetPendingApartmentsQuery : IRequest<Response<PendingApartmentsResponse>>
    {
    }
}
