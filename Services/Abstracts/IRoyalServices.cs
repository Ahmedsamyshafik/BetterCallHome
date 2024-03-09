using Domin.Models;

namespace Services.Abstracts
{
    public interface IRoyalServices
    {
        Task<RoyalDocument> AddRoyal(string Royalname, string RoyalPath, int apartmentID);
    }
}
