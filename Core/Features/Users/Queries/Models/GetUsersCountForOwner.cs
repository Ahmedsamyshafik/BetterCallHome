using Core.Bases;
using Core.Features.Users.Queries.Results;
using MediatR;

namespace Core.Features.Users.Queries.Models
{
    public class GetUsersCountForOwner : IRequest<Response<GetUsersCountForOwnerResponse>>
    {
        public string OwnerId { get; set; }
    }
}
