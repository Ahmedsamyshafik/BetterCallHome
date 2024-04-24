using Domin.Constant;
using Domin.Models;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;

namespace Services.Implementations
{
    public class VideosServices : IVideosServices
    {
        #region Fields
        private readonly IVideoRepository _video;

        #endregion

        #region Ctor
        public VideosServices(IVideoRepository video)
        {
            _video = video;
        }

        #endregion

        #region Handle functions

        public async Task<ApartmentVideo> AddVideo(string url, int apartmentID, string name)
        {
            ApartmentVideo video = new ApartmentVideo() { ApartmentID = apartmentID, VideoUrl = url, Name = name };
            return await _video.AddAsync(video);
        }
        public async Task<string> DeleteVideo(int aprtmentId)
        {
            var video = _video.GetTableNoTracking().Where(x => x.ApartmentID == aprtmentId).FirstOrDefault();
            if (video != null)
            {
                await _video.DeleteAsync(video);
            }

            return "";
        }

        public async Task<string> DeleteApartmentVideoFile(int apartmentId)
        {
            var image = _video.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId)
                .Include(x => x.Apartment).FirstOrDefault();
            if (image != null)
            {
                var r = Path.Combine("wwwroot", Constants.ApartmentVids, image.Name); //Dic
                if (File.Exists(r)) File.Delete(r);
            }

            return "";
        }

        public List<ApartmentVideo> GetAll()
        {
            return _video.GetTableNoTracking().ToList();
        }

        public List<string> GetApartmentdVideo(int apartmentId)
        {
            var videos = _video.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId).ToList();
            var response = new List<string>();
            foreach (var video in videos)
            {
                response.Add(video.VideoUrl);
            }
            return response;
        }
        #endregion


    }
}
