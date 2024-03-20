using Domin.Models;

namespace Services.Abstracts
{
    public interface IRoyalServices
    {
        Task<RoyalDocument> AddRoyal(string RoyalUrl, int apartmentID, string name);
        Task<string> DeleteApartmentDocumentFile(int apartmentId);
        List<RoyalDocument> GetAll();
    }
}
