using Domin.Models;

namespace Services.Abstracts
{
    public interface IVideosServices
    {
        Task<ApartmentVideo> AddVideo(string url, int apartmentID, string name);
        Task<string> DeleteApartmentVideoFile(int apartmentId);
        List<string> GetApartmentdVideo(int apartmentId);
        List<ApartmentVideo> GetAll();
    }
}
