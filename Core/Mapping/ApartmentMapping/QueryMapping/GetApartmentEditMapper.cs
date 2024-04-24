
using AutoMapper;
using Core.Features.Apartments.Queries.Results;
using Domin.Models;

namespace Core.Mapping.ApartmentMapping
{
    public partial class ApartmentProfile : Profile
    {
        public void GetApartmentEditMapper()
        {
            CreateMap<Apartment, GetApartmentForEditResponse>().
                ForMember(dest => dest.ApartmentName, opt => opt.MapFrom(src => src.Name)).
                ForMember(dest => dest.ApartmentId, opt => opt.MapFrom(src => src.Id)).
                ForMember(dest => dest.ApartmentCoverImage, opt => opt.MapFrom(src => src.CoverImageUrl)).
                ForMember(dest => dest.ApartmentVideo, opt => opt.MapFrom(src => src.ApartmentVideo)).
                ForMember(dest => dest.ApartmentAddress, opt => opt.MapFrom(src => src.Address)).
                ForMember(dest => dest.ApartmentDescription, opt => opt.MapFrom(src => src.Description)).
                ForMember(dest => dest.ApartmentUsers, opt => opt.MapFrom(src => src.NumberOfUsers)).
                ForMember(dest => dest.ApartmentRooms, opt => opt.MapFrom(src => src.Room)).
                ForMember(dest => dest.ApartmentRooms, opt => opt.MapFrom(src => src.Room)).
                ForMember(dest => dest.ApartmentPrice, opt => opt.MapFrom(src => src.Price)).
                ForMember(dest => dest.ApartmentGender, opt => opt.MapFrom(src => src.gender)).
                ForMember(dest => dest.ApartmentCity, opt => opt.MapFrom(src => src.City));
        }
        // //Apartment=>GetApartmentForEditRespons  GetApartmentEditMapper
    }
}
