using Domin.Models;

namespace Services.Abstracts
{
    public interface IVideosServices
    {
        Task<ApartmentVideo> AddVideo(string url, int apartmentID);
    }
}
