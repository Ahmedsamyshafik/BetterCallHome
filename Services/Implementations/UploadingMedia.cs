using Infrastructure.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Abstracts;

namespace Services.Implementations
{
    public class UploadingMedia : IUploadingMedia
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UploadingMedia(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<ReturnedMediaDto> UploadFileAsync(IFormFile media, string folderPath)
        {

            var ImagePath = $"{_hostingEnvironment.WebRootPath}{folderPath}";
            string uniqueFileName = Guid.NewGuid().ToString() + media.FileName;
            var uploadsFolder = Path.Combine(folderPath, uniqueFileName);
            uploadsFolder = Path.Combine(ImagePath, uniqueFileName);
            using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
            {
                await media.CopyToAsync(fileStream);
            }
            var result=new ReturnedMediaDto() { Name=uniqueFileName , Path=ImagePath};
            return result;

        }

        public async Task<ICollection<ReturnedMediaDto>> UploadFilesAsync(ICollection<IFormFile> media, string folderPath)
        {
            ICollection<ReturnedMediaDto> pathes = new List<ReturnedMediaDto>();
            foreach(var file in media)
            {
                var ImagePath = $"{_hostingEnvironment.WebRootPath}{folderPath}";
                string uniqueFileName = Guid.NewGuid().ToString() + file.FileName;
                var uploadsFolder = Path.Combine(folderPath, uniqueFileName);
                uploadsFolder = Path.Combine(ImagePath, uniqueFileName);
                using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var result=new ReturnedMediaDto() { Name=uniqueFileName , Path=ImagePath};
                pathes.Add(result);
            }
            return pathes;
          
        }
    }
}
