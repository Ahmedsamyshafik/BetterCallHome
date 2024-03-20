using Core.Bases;
using Core.Wrappers;
using Infrastructure.DTO;
using MediatR;

namespace Core.Features.Users.Queries.Models
{
    public class GetAllUserQuery : IRequest<PaginatedResult<GetAllUsersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
