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
                ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City)).
               ForMember(dest => dest.gender, opt => opt.MapFrom(src => src.Gender)).
                ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt)).
                ForMember(dest => dest.Document, opt => opt.Ignore()).
                ForMember(dest => dest.ApartmentVideo, opt => opt.Ignore()).
                ForMember(dest => dest.CoverImageName, opt => opt.Ignore()).
                ForMember(dest => dest.RoyalDocument, opt => opt.Ignore());
        }
        public void EditApartmentCommandMapping()
        {

            //from EditApartmentCommand => Apartment
            CreateMap<EditApartmentCommand, Apartment>().
                ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title)).
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApartmentId)).
                ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
        }
    }
}
