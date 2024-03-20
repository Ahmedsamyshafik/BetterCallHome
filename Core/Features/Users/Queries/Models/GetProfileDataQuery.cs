using Core.Bases;
using Core.Features.Users.Queries.Results;
using MediatR;

namespace Core.Features.Users.Queries.Models
{
    public class GetProfileDataQuery : IRequest<Response<UserDataResponse>>
    {
        public string RequestedId { get; set; }
        public string? RequesterUserID { get; set; }

    }
}
