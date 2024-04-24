using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.DTO.Apartments.Detail;
using infrustructure.DTO.Apartments.Pagination;
using Microsoft.AspNetCore.Http;

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
        IQueryable<ApartmentPaginationPending> getPendingpaginate(string? search);
        Task<string> HandlePendingApartments(bool Accept, Apartment apartment);

        IQueryable<GetPendingApartmentsForOwnerPaginationDTO> getpaginateForOwner(string ownerID, string? search);
        Task<string> AssingStudnetsToApartment(int apartmentId);
        Task<string> EditApartment(Apartment apartment, IFormFile? CoverImage, IFormFile? Video, List<IFormFile>? Pics,
            string requestSchema, HostString host);

        IQueryable<GetApartmentPagintationResponse> getApartmentspaginate(string? search, string? city, string? gender,
         int countIn, decimal? min, decimal? max);

        Task<GetApartmentDetailResponseDTO> GetApartmentDetails(int ApartmentId);
        List<Apartment> GetApartmentsTopRateForLandingPage();

    }
}
