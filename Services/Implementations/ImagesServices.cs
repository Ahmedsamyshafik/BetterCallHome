using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ImagesServices : IImagesServices
    {
        private readonly IimagesRepository _iimages;
        public ImagesServices(IimagesRepository iimages) { _iimages = iimages; }

        public async Task<string> AddImage(string imgUrl, int apartmentID)
        {

            ApartmentImages apartmentImages = new ApartmentImages() { ApartmentID = apartmentID, ImageUrl = imgUrl };
            var x = await _iimages.AddAsync(apartmentImages);
            if (x == null) return "Faild";
            return "Success";
        }
    }
}
