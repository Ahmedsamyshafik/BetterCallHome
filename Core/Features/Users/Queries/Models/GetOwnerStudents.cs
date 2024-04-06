using Core.Bases;
using Core.Features.Users.Queries.Results;
using Core.Wrappers;
using Infrastructure.DTO.Owner;
using MediatR;

namespace Core.Features.Users.Queries.Models
{
    public class GetOwnerStudents : IRequest<PaginatedResult<GetOwnerStudentsResponse>>
    {
        public string OwnerID { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
