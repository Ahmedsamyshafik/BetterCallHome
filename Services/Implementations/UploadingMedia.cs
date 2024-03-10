using Domin.Models;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;

namespace Services.Implementations
{
    public class UploadingMedia : IUploadingMedia
    {
        #region Fields
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Ctor
        public UploadingMedia(IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region Handle Functions


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
            var result = new ReturnedMediaDto() { Name = uniqueFileName, Path = ImagePath };
            return result;

        }

        public async Task<ICollection<ReturnedMediaDto>> UploadFilesAsync(ICollection<IFormFile> media, string folderPath)
        {
            ICollection<ReturnedMediaDto> pathes = new List<ReturnedMediaDto>();
            foreach (var file in media)
            {
                var ImagePath = $"{_hostingEnvironment.WebRootPath}{folderPath}";
                string uniqueFileName = Guid.NewGuid().ToString() + file.FileName;
                var uploadsFolder = Path.Combine(folderPath, uniqueFileName);
                uploadsFolder = Path.Combine(ImagePath, uniqueFileName);
                using (var fileStream = new FileStream(uploadsFolder, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var result = new ReturnedMediaDto() { Name = uniqueFileName, Path = ImagePath };
                pathes.Add(result);
            }
            return pathes;

        }

        public async Task<IFormFile> GetImage(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            string imageUser = user.imagePath;
            string imageName = user.imageName;
            string path = $"{imageUser}/{imageName}";
            var bytes = System.IO.File.ReadAllBytes(path);
            string contentType = GetContentType(imageName);
            // Return the file with the appropriate content type
            //var x = System.IO.File(bytes, contentType, Path.GetFileName(path));
            //IFormFile file = new FormFile(File.OpenRead(path), 0, new FileInfo(path).Length, "fileName", imageName)
            //{
            //    Headers = new HeaderDictionary(),
            //    ContentType = contentType
            //};

            //using (var stream = new FileStream(path, FileMode.Open))
            //{
            //    // Create an IFormFile instance
            //    var file = new FormFile(stream, 0, stream.Length, "fileName", imageName)
            //    {
            //        Headers = new HeaderDictionary(),
            //        ContentType = contentType
            //    };

            //    var zehket=File.OpenRead(path);
            //    var ahh = 2;
            //    // Process the file...
            //    return file;
            //}

            //using (MemoryStream stream = new MemoryStream(bytes))
            //{
            //    IFormFile file = new FormFile(stream, 0, stream.Length, "fileName", "jpg")
            //    {
            //        Headers = new HeaderDictionary(),
            //        ContentType = contentType
            //    };
            //    return file;

            //    Use the "file" object as needed
            //}

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                // Create an IFormFile from the MemoryStream
                IFormFile imageFile = new FormFile(memoryStream, 0, bytes.Length, "file", imageName);

                return imageFile;
            }





            //var x = File(bytes, contentType, Path.GetFileName(path));




        }
        public byte[] GetImageFile(byte[] imageBytes)
        {
            // Check if the byte array is not null or empty
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentNullException("imageBytes", "Image byte array cannot be null or empty.");
            }

            // You can save the byte array to a temporary file
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllBytes(tempFilePath, imageBytes);

            // Return the path to the temporary file
            return File.ReadAllBytes(tempFilePath);
        }

        //public async Task<IFormFile> GetImage(string userid)
        //{
        //    var user = await _userManager.FindByIdAsync(userid);
        //    string imageUser = user.imagePath;
        //    string imageName = user.imageName;
        //    string paths = $"{imageUser}/{imageName}";
        //    // Read the file into a byte array
        //    var bytes = File.ReadAllBytes(paths);
        //    string contentType = GetContentType(imageName);
        //    // Return the file with the appropriate content type
        //    return System.IO.File(bytes, contentType, Path.GetFileName(paths));
        //}

        private string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            string contentType;

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                // Add more cases for other file extensions if needed
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return contentType;
        }


        #endregion






    }
}
