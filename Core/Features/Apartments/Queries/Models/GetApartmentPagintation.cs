using Core.Features.Apartments.Queries.Results;
using Core.Wrappers;
using infrustructure.DTO.Apartments.Pagination;
using MediatR;

namespace Core.Features.Apartments.Queries.Models
{
    public class GetApartmentPagintation : IRequest<PaginatedResult<GetApartmentPagintationResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
        public int CountInApartment { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }

    }
}
