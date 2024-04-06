using Domin.Constant;
using Domin.Models;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
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
        public async Task<RoyalDocument> AddRoyal(string RoyalUrl, int apartmentID, string name)
        {
            RoyalDocument document = new RoyalDocument() { ApartmentID = apartmentID, ImageUrl = RoyalUrl, Name = name };
            return await _royal.AddAsync(document);

        }

        public async Task<string> DeleteApartmentDocumentFile(int apartmentId)
        {
            var image = _royal.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId)
                .Include(x => x.Apartment).FirstOrDefault();
            if (image != null)
            {
                var r = Path.Combine("wwwroot", Constants.ApartmentRoyalDocPics, image.Name); //Dic
                if (File.Exists(r)) File.Delete(r);
            }

            return "";
        }

        public List<RoyalDocument> GetAll()
        {
            return _royal.GetTableNoTracking().ToList();
        }
    }
}
