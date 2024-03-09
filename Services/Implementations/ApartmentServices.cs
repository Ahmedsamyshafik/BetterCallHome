using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ApartmentServices : IApartmentServices
    {
        private readonly IApartmentRepository _apartmentRepository;
        public ApartmentServices(IApartmentRepository apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        public async Task<Apartment> AddApartmentAsync(Apartment apartment)
        {
           return await _apartmentRepository.AddAsync(apartment);
            
        }

        public async Task<string> UpdateApartmentAsync(Apartment apartment)
        {
            await _apartmentRepository.UpdateAsync(apartment);
            return "";
        }
    }
}
