using AutoMapper;
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
    }
}
