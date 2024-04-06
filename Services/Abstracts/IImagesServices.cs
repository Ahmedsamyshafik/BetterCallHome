using Domin.Models;

namespace Services.Abstracts
{
    public interface IImagesServices
    {
        Task<string> AddImage(string imgUrl, int apartmentID, string name);
        Task<string> DeleteApartmentImageFile(int apartmentId);
        string DeleteAccountPic(string imageName);
        List<ApartmentImages> GetApartmentImgs(int apartmentId);
        List<ApartmentImages> GetAlll();
        Task<string> DeleteApartmentCoverImage(int apartmentId);
        Task<string> DeleteApartmentPics(int apartmentId);
    }
}
