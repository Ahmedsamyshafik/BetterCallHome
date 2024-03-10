using Domin.Models;

namespace Services.Abstracts
{
    public interface IApartmentServices
    {
        Task<Apartment> AddApartmentAsync(Apartment apartment);
        Task<string> UpdateApartmentAsync(Apartment apartment);
        Task<Apartment> GetApartment(int apartmentId);
        IQueryable<Apartment> GetUserApartments(string userID);
    }
}
