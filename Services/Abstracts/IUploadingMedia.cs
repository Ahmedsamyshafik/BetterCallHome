using Microsoft.AspNetCore.Http;
using static Services.Implementations.UploadingMedia;

namespace Services.Abstracts
{
    public interface IUploadingMedia
    {
        //Task<ReturnedMediaDto> UploadFileAsync(IFormFile media, string folderPath);
        //Task<ICollection<ReturnedMediaDto>> UploadFilesAsync(ICollection<IFormFile> media, string folderPath);
        ////  Task<IFormFile> GetImage(string userId);
        Task<responsing> SavingImage(IFormFile imageFile, string requestSchema, HostString hostString, string folderName);
    }
}
