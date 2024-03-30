using Core.Features.Apartments.Queries.Results;
using Core.Wrappers;
using MediatR;

namespace Core.Features.Apartments.Queries.Models
{
    public class GetApartmentsForOwnerQuery  : IRequest<PaginatedResult<GetPendingApartmentsForOwnerPaginationResponse>>
    {
        public string OwnerId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
