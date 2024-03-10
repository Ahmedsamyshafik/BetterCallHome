using Core.Bases;
using Core.Features.Apartments.Queries.Results;
using MediatR;

namespace Core.Features.Apartments.Queries.Models
{
    public class GetNotificationApartmentQuery : IRequest<Response<NotificationApartmentResponse>>
    {
        public string UserId { get; set; }
        public GetNotificationApartmentQuery(string userId)
        {
            UserId = userId;
        }
    }
}
