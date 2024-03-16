using Domin.Models;
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

        public class responsing
        {
            public bool success;
            public string message;
        }

        public async Task<responsing> SavingImage(IFormFile imageFile, string requestSchema, HostString hostString, string folderName)
        {
            string _uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {

                    return new responsing() { success = false, message = "Please provide a valid image file." };

                }

                // Ensure the uploads directory exists
                Directory.CreateDirectory(_uploadsDirectory);

                // Generate a unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                // Combine the uploads directory with the unique file name
                var filePath = Path.Combine(_uploadsDirectory, uniqueFileName);

                // Save the uploaded file to the uploads directory
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                // Construct the URL to access the uploaded file
                //  var imageUrl = $"{Request.Scheme}://{Request.Host}/uploads/{uniqueFileName}";
                var imageUrl = $"{requestSchema}://{hostString}/{folderName}/{uniqueFileName}";


                // Return the URL of the uploaded image
                return new responsing() { success = true, message = imageUrl };

            }
            catch (Exception ex)
            {
                return new responsing() { success = false, message = $"An error occurred: {ex.Message}" };


            }
        }

        #endregion



    }

}


