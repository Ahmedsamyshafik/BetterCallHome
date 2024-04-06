using Core.Features.Users.Queries.Results;
using Core.Wrappers;
using Infrastructure.DTO.Student;
using MediatR;

namespace Core.Features.Users.Queries.Models
{
    public class GetApartmentsRequestsQuery :IRequest<PaginatedResult<ApartmentRequestsResponse>>
    {
        public string OwnerId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
