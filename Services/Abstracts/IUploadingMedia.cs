using Microsoft.AspNetCore.Http;
using static Services.Implementations.UploadingMedia;

namespace Services.Abstracts
{
    public interface IUploadingMedia
    {

        Task<responsing> SavingImage(IFormFile imageFile, string requestSchema, HostString hostString, string folderName);
    }
}
