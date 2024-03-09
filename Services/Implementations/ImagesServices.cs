using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ImagesServices : IImagesServices
    {
        private readonly IimagesRepository _iimages;
        public ImagesServices(IimagesRepository iimages) { _iimages = iimages; }
        public async Task<string> AddImage(string imgname, string imgPath, int apartmentID)
        {

            ApartmentImages apartmentImages = new ApartmentImages() { ApartmentID = apartmentID, ImageName = imgname, ImagePath = imgPath };
            var x = await _iimages.AddAsync(apartmentImages);
            if (x == null) return "Faild";
            return "Success";
        }
    }
}
