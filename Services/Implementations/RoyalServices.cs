using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class RoyalServices : IRoyalServices
    {
        private readonly IRoyalDocumentRepository _royal;
        public RoyalServices(IRoyalDocumentRepository royal)
        {
            _royal = royal;
        }
        public async Task<RoyalDocument> AddRoyal(string RoyalUrl, int apartmentID)
        {
            RoyalDocument document = new RoyalDocument() { ApartmentID = apartmentID, ImageUrl = RoyalUrl };
            return await _royal.AddAsync(document);

        }
    }
}
