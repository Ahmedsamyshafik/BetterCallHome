using Domin.Models;

namespace Services.Abstracts
{
    public interface IVideosServices
    {
        Task<ApartmentVideo> AddVideo(string videoname, string videoPath, int apartmentID);
    }
}
