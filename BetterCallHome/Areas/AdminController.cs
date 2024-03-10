using BetterCallHomeWeb.Base;
using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Abstracts;

namespace BetterCallHomeWeb.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Area("Admin")]
    public class AdminController : AppControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUploadingMedia _uploadingMedia;
        public AdminController(UserManager<ApplicationUser> userManager, IUploadingMedia uploadingMedia)
        {
            _userManager = userManager;
            _uploadingMedia = uploadingMedia;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> getimage(string userId)
        {
            var x = _uploadingMedia.GetImage(userId);

            //var user = await _userManager.FindByIdAsync(userId);
            //string imageUser = user.imagePath;
            //string imageName = user.imageName;
            //string path = $"{imageUser}/{imageName}";
            //var bytes = System.IO.File.ReadAllBytes(path);
            //string contentType = GetContentType(imageName);
            ////Return the file with the appropriate content type
            //var x = File(bytes, contentType, Path.GetFileName(path));
            return Ok(new { strings = "Hello", imageFile = x });

            //IFormFile file = new FormFile(System.IO.File.OpenRead(path), 0, new FileInfo(path).Length, imageUser, imageName);
            //   IFormFile file1=new FormFile();
            // IFormFile file = new FormFile
            //           (stream, 0, stream.Length, imageUser, Path.GetFileName(paths));
            //  return System.IO.File(bytes, contentType, Path.GetFileName(paths));

        }



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


    }
}
