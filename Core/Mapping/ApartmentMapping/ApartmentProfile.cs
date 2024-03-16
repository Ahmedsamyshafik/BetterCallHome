using AutoMapper;

namespace Core.Mapping.ApartmentMapping
{
    public partial class ApartmentProfile : Profile
    {
        public ApartmentProfile()
        {
            AddApartmentCommandMapping();
            GetPendingApartmentQueryMapping();

        }
    }
}
