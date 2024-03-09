using AutoMapper;
using Core.Bases;
using Core.Features.Apartments.Commands.Models;
using Domin.Constant;
using Domin.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;

namespace Core.Features.Apartments.Commands.Handlers
{
    public class ApartmentCommandHandler : ResponseHandler,
                    IRequestHandler<AddApartmentCommand, Response<string>>
    {
        #region InJect
        private readonly IApartmentServices _apartmentServices;
        private readonly IMapper _mapper;
        private readonly IUploadingMedia _media;
        private readonly IVideosServices _videos;
        private readonly IImagesServices _images;
        private readonly IRoyalServices _royal;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApartmentCommandHandler(IApartmentServices apartmentServices, IMapper mapper, IUploadingMedia media
             , IVideosServices videos, IImagesServices images, IRoyalServices royal, UserManager<ApplicationUser> userManager)
        {
            _apartmentServices = apartmentServices;
            _mapper = mapper;
            _media = media;
            _videos = videos;
            _images = images;
            _royal = royal;
            _userManager = userManager;

        }
        #endregion


        public async Task<Response<string>> Handle(AddApartmentCommand request, CancellationToken cancellationToken)
        {
            //valid userID!
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return BadRequest<string>("User Not Found!");

            //mapping from AddApartmentCommand To Apartment
            var mapper = _mapper.Map<Apartment>(request);
            //Service To Add Apartment
            try
            {
                // Adding Apartment to get (ApartmentID)
                var apart = await _apartmentServices.AddApartmentAsync(mapper);
                //Services To Save this imges and videos..
                if (request.video != null)
                {
                    var videoPath = await _media.UploadFileAsync(request.video, Constants.ApartmentVids);
                    var x = await _videos.AddVideo(videoPath.Name, videoPath.Path, apart.Id);
                    apart.ApartmentVideoID = x.Id;
                }
                if (request.Pics != null)
                {
                    var PicsPaths = await _media.UploadFilesAsync(request.Pics, Constants.ApartmentPics);
                    foreach (var p in PicsPaths)
                    {
                        await _images.AddImage(p.Name, p.Path, apart.Id);
                    }// Cover!
                    if (request.CoverImage != null)
                    {
                        var CoverImage = await _media.UploadFileAsync(request.CoverImage, Constants.ApartmentPics);
                        apart.CoverImageName = CoverImage.Name;
                    }
                }
                if (request.RoyalDocument != null)
                {
                    var RoyalPath = await _media.UploadFileAsync(request.RoyalDocument, Constants.ApartmentRoyalDocPics);
                    var x = await _royal.AddRoyal(RoyalPath.Name, RoyalPath.Path, apart.Id);
                    apart.Document = x.Id;
                }
                await _apartmentServices.UpdateApartmentAsync(apart);
                return Success("");
            }
            catch
            {
                return BadRequest<string>("Faild");
            }
        }
    }
}
