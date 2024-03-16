using Domin.Models;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ApartmentServices : IApartmentServices
    {
        #region Fields
        private readonly IApartmentRepository _apartmentRepository;

        #endregion

        #region Ctor
        public ApartmentServices(IApartmentRepository apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<Apartment> AddApartmentAsync(Apartment apartment)
        {
            return await _apartmentRepository.AddAsync(apartment);

        }

        public async Task<string> UpdateApartmentAsync(Apartment apartment)
        {
            await _apartmentRepository.UpdateAsync(apartment);
            return "";
        }

        public async Task<Apartment> GetApartment(int apartmentId)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(apartmentId);
            return apartment;
        }

        public IQueryable<Apartment> GetOwnerApartments(string userID)
        {
            var apartments = _apartmentRepository.GetTableNoTracking().Where(x => x.OwnerId == userID);
            return apartments;
        }

        public async Task<List<Apartment>> GetPendingApartmentd()
        {
            return await _apartmentRepository.GetTableNoTracking().Where(x => x.Publish == false).ToListAsync();
        }
        #endregion



    }
}
