using Core.Bases;
using Core.Features.Apartments.Queries.Results;
using Infrastructure.DTO.Apartments.Detail;
using MediatR;

namespace Core.Features.Apartments.Queries.Models
{
    public class GetApartmentDetailQuery :IRequest<Response<GetApartmentDetailResponse>>
    {
        public int Id { get; set; } // Comments Pagination
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
