using Domin.Models;

namespace Services.Abstracts
{
    public interface IRoyalServices
    {
        Task<RoyalDocument> AddRoyal(string RoyalUrl, int apartmentID);
    }
}
