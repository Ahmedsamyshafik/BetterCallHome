using AutoMapper;
using Core.Features.Apartments.Queries.Results;
using Infrastructure.DTO;

namespace Core.Mapping.ApartmentMapping
{
    public partial class ApartmentProfile : Profile
    {
        public void GetPendingApartmentQueryMapping()
        {

            CreateMap<ApartmentPaginationPending, GetPendingApartmentsPaginationResponse>();
        }
        public void GetApartmentsForOwnerQueryMapping()
        
        {

            CreateMap<GetPendingApartmentsForOwnerPaginationDTO, GetPendingApartmentsForOwnerPaginationResponse>()
                .ForMember(d=>d.Description,opt=>opt.MapFrom(s=>s.Description));
        }
    }
}
