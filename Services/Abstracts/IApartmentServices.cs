using Domin.Models;

namespace Services.Abstracts
{
    public interface IApartmentServices
    {
        Task<Apartment> AddApartmentAsync(Apartment apartment);
        Task<string> UpdateApartmentAsync(Apartment apartment);
    }
}
