using Domin.Constant;
using Domin.Models;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ImagesServices : IImagesServices
    {
        private readonly IimagesRepository _iimages;
        public ImagesServices(IimagesRepository iimages) { _iimages = iimages; }

        public async Task<string> AddImage(string imgUrl, int apartmentID, string name)
        {

            ApartmentImages apartmentImages = new ApartmentImages() { ApartmentID = apartmentID, ImageUrl = imgUrl, Name = name };
            var x = await _iimages.AddAsync(apartmentImages);
            if (x == null) return "Faild";
            return "Success";
        }

        public string DeleteAccountPic(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                var r = Path.Combine("wwwroot", Constants.EditProfilePicture, imageName); //pics
                if (File.Exists(r)) File.Delete(r);
            }

            return "";
        }

        public async Task<string> DeleteApartmentImageFile(int apartmentId)
        {
            var images = _iimages.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId)
                .Include(x => x.Apartment);

            if (images.Count() > 0)
            {
                foreach (var image in images)
                {
                    var r = Path.Combine("wwwroot", Constants.ApartmentPics, image.Name); //pics
                    if (File.Exists(r)) File.Delete(r);

                    r = Path.Combine("wwwroot", Constants.ApartmentPics, image.Apartment.CoverImageName); //Cover Image
                    if (File.Exists(r)) File.Delete(r);
                }
            }
            return "";
        }
        public async Task<string> DeleteApartmentCoverImage(int apartmentId )
        {
            var images = _iimages.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId)
               .Include(x => x.Apartment);
            

            if (images.Count() > 0)
            {
                foreach (var image in images)
                { 
                    var r = Path.Combine("wwwroot", Constants.ApartmentPics, image.Apartment.CoverImageName); //Cover Image
                    if (File.Exists(r)) File.Delete(r);
                }
            }
            return "";
        }
        public async Task<string> DeleteApartmentPics(int apartmentId)
        {
            var images = _iimages.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId)
               .Include(x => x.Apartment);


            if (images.Count() > 0)
            {
                foreach (var image in images)
                {
                    var r = Path.Combine("wwwroot", Constants.ApartmentPics, image.Name); //pics
                    if (File.Exists(r)) File.Delete(r);
                }
            }
            return "";
        }

        public List<ApartmentImages> GetAlll()
        {
            return _iimages.GetTableNoTracking().ToList();
        }

        public List<ApartmentImages> GetApartmentImgs(int apartmentId)
        {

            //try
            //{
            //    _iimages.BeginTransaction();
            //    var imgs = _iimages.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId);
            //    _iimages.Commit();
            //    var response = new List<string>();
            //    for (int i = 0; i < imgs.AsEnumerable().Count(); i++)
            //    {
            //        response.Add(imgs.ElementAt(i).ImageUrl);
            //    }

            //    List<ApartmentImages> apartments = imgs.ToList(); //for i
            //    var x = apartments.Count();
            //    foreach (var img in imgs)
            //    {
            //        response.Add(img.ImageUrl);
            //    }
            //    return response;
            //}
            //catch
            //{
            //    var imgs = _iimages.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId).AsQueryable();
            //    // .ToList();
            //    var response = new List<string>();
            //    //List<ApartmentImages> apartments = imgs.ToList();
            //    //var x = apartments.Count();
            //    foreach (var img in imgs)
            //    {
            //        response.Add(img.ImageUrl);
            //    }
            //    return response;
            //}


            var images = _iimages.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId)
               .Include(x => x.Apartment);
            var listOfApartmentImages = new List<ApartmentImages>();
            if (images.Count() > 0)
            {
                listOfApartmentImages.AddRange(images);
            }
            return listOfApartmentImages;
            //           var imgs = _iimages.GetTableNoTracking().AsQueryable();
            // var x = _iimages.GetTableNoTracking().ToList(); .. Error!

        }
    }
}
