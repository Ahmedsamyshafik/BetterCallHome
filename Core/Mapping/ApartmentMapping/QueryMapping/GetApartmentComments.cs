
using AutoMapper;
using Core.Features.Apartments.Queries.Results;
using Infrastructure.DTO.Apartments.Detail;

namespace Core.Mapping.ApartmentMapping
{
    public partial class ApartmentProfile : Profile
    {
        public void GetApartmentComments()
        {
            //ApartmentComments
            CreateMap<ApartmentComments, ApartmentCommentsResponse>();

        }
        public void GetApartmentDetailsDTOToResponse()
        {
            CreateMap<GetApartmentDetailResponseDTO, GetApartmentDetailResponse>().
                ForMember(dest => dest.ApartmentComments, opt => opt.Ignore());
        }

    }
}
