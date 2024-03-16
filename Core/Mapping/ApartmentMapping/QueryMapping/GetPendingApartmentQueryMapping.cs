using AutoMapper;
using Core.Features.Apartments.Queries.Results;

namespace Core.Mapping.ApartmentMapping
{
    public partial class ApartmentProfile : Profile
    {
        public void GetPendingApartmentQueryMapping()
        {

            CreateMap<TempMappingApartmentsOwnersToPendint, PendingCollection>().
                ForMember(dest => dest.ApartmentCoverImage, opt => opt.MapFrom(src => src.Apartment.CoverImageName)).
                ForMember(dest => dest.ApartmentTitle, opt => opt.MapFrom(src => src.Apartment.Name)).
                ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src =>
                    src.Apartment.OwnerId.Equals(src.user.Id) ? src.user.UserName : null
                )).
                ForMember(dest => dest.OwnerImage, opt => opt.MapFrom(src =>
                 src.Apartment.OwnerId.Equals(src.user.Id) ? src.user.imageUrl : null
                )).
                ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Apartment.Address)).
                ForMember(dest => dest.NumberOfUsers, opt => opt.MapFrom(src => src.Apartment.NumberOfUsers)).
                ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.Apartment.CreatedAt)).
                ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Apartment.Price));


        }
    }
}
