using AutoMapper;
using Core.Features.Apartments.Commands.Models;
using Domin.Models;

namespace Core.Mapping.ApartmentMapping
{
    public partial class ApartmentProfile : Profile
    {
        public void AddApartmentCommandMapping()
        {        //mapping from AddApartmentCommand To Apartment
            CreateMap<AddApartmentCommand, Apartment>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId)).
                ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)).
                ForMember(dest => dest.Document, opt => opt.Ignore()).
                ForMember(dest => dest.ApartmentVideo, opt => opt.Ignore()).
                ForMember(dest => dest.CoverImageName, opt => opt.Ignore()).
                ForMember(dest => dest.RoyalDocument, opt => opt.Ignore());
        }
    }
}
