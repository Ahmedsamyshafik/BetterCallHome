using Core.Bases;
using Core.Features.Apartments.Queries.Results;
using Core.Wrappers;
using MediatR;

namespace Core.Features.Apartments.Queries.Models
{
    public class GetPendingApartmentsQuery : IRequest<PaginatedResult<GetPendingApartmentsPaginationResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
