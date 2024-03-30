using Domin.Models;
using Infrastructure.DTO;

namespace Services.Abstracts
{
    public interface IApartmentServices
    {
        Task<Apartment> AddApartmentAsync(Apartment apartment);
        Task<string> UpdateApartmentAsync(Apartment apartment);
        Task<string> DeleteApartmentFilesOnly(Apartment apartment);
        Task<string> DeleteApartmentAsync(Apartment apartment);
        Task<Apartment> GetApartment(int apartmentId);
        Task<List<Apartment>> GetOwnerApartmentsAsList(string userID);
        IQueryable<Apartment> GetOwnerApartments(string userID);
        IQueryable<Apartment> GetPendingApartmentd(string? search);
        //Task<List<ApartmentPaginationPending>> getpaginate(string? search);
        IQueryable<ApartmentPaginationPending> getpaginate(string? search);
        Task<string> HandlePendingApartments(bool Accept, Apartment apartment);

        IQueryable<GetPendingApartmentsForOwnerPaginationDTO> getpaginateForOwner(string ownerID, string? search);

    }
}
