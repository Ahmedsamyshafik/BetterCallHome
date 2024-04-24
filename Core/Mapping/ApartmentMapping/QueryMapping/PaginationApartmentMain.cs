using AutoMapper;
using Core.Features.Apartments.Queries.Results;
using Domin.Models;
using infrustructure.DTO.Apartments.Pagination;

namespace Core.Mapping.ApartmentMapping
{
    public partial class ApartmentProfile : Profile
    {
        public void PaginationApartmentMain()
        {
            //GetApartmentPagintationResponse - 
            CreateMap<GetApartmentPagintationResponse, GetApartmentPagintationResponse>();
        }
        public void GetApartmentTopRate()
        {
            CreateMap<Apartment, GetApartmentLandingPageResponse>().
                ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id)).
                ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.CoverImageUrl));
        }
    }
}
