using Infrastructure.DTO;
using Microsoft.AspNetCore.Http;

namespace Services.Abstracts
{
    public interface IUploadingMedia
    {
        Task<ReturnedMediaDto> UploadFileAsync(IFormFile media, string folderPath);
        Task<ICollection<ReturnedMediaDto>> UploadFilesAsync(ICollection<IFormFile> media, string folderPath);
        Task<IFormFile> GetImage(string userId);
    }
}
