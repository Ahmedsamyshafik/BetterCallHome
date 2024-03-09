using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class VideosServices : IVideosServices
    {
        private readonly IVideoRepository _video;
        public VideosServices(IVideoRepository video)
        {
            _video = video;
        }
        public async Task<ApartmentVideo> AddVideo(string videoname, string videoPath, int apartmentID)
        {
            ApartmentVideo video = new ApartmentVideo() { ApartmentID = apartmentID, VideoName = videoname, VideoPath = videoPath };
            return await _video.AddAsync(video);
        }
    }
}
