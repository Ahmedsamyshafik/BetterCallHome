using AutoMapper;
using Core.Bases;
using Core.Features.Apartments.Commands.Models;
using Core.Features.Apartments.Commands.Results;
using Domin.Constant;
using Domin.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;
using Services.Implementations;

namespace Core.Features.Apartments.Commands.Handlers
{
    public class ApartmentCommandHandler : ResponseHandler,
                    IRequestHandler<AddApartmentCommand, Response<AddApartmentResponse>>,
                    IRequestHandler<AddCommentApartmentCommand, Response<string>>,
                    IRequestHandler<AddReactApartmentCommand, Response<string>>,
                    IRequestHandler<PendingApartmentAction, Response<string>>,
                    IRequestHandler<DeleteApartmentCommand, Response<string>>,
                    IRequestHandler<EditApartmentCommand, Response<string>>
    {
        #region Fields
        private readonly IApartmentServices _apartmentServices;
        private readonly IMapper _mapper;
        private readonly IUploadingMedia _media;
        private readonly IVideosServices _videos;
        private readonly IImagesServices _images;
        private readonly IRoyalServices _royal;
        private readonly ICommentServices _comment;
        private readonly IReactServices _react;
        private readonly IUsersApartmentsServices _usersApartments;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion
        #region Ctor
        public ApartmentCommandHandler(IApartmentServices apartmentServices, IMapper mapper, IUploadingMedia media
                    , IVideosServices videos, IImagesServices images, IRoyalServices royal, UserManager<ApplicationUser> userManager
                   , ICommentServices comment, IReactServices react, IUsersApartmentsServices usersApartments)
        {
            _apartmentServices = apartmentServices;
            _mapper = mapper;
            _media = media;
            _videos = videos;
            _images = images;
            _royal = royal;
            _userManager = userManager;
            _comment = comment;
            _react = react;
            _usersApartments = usersApartments;
        }
        #endregion


        #region Handle Functions

        public async Task<Response<AddApartmentResponse>> Handle(AddApartmentCommand request, CancellationToken cancellationToken)
        {
            //valid userID!
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return BadRequest<AddApartmentResponse>("User Not Found!");
            if (!user.EmailConfirmed) return BadRequest<AddApartmentResponse>("Not Confirmed!");
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
                    var videoPath = await _media.SavingImage(request.video, request.RequestScheme, request.Requesthost, Constants.ApartmentVids);
                    if (!videoPath.success) return BadRequest<AddApartmentResponse>("Faild in saving video");
                    var x = await _videos.AddVideo(videoPath.message, apart.Id, videoPath.name);
                    apart.ApartmentVideoID = x.Id;
                }
                if (request.Pics != null)
                {

                    foreach (var Pic in request.Pics)
                    {
                        if (Pic.Length > 0)
                        {
                            var PicsPaths = await _media.SavingImage(Pic, request.RequestScheme, request.Requesthost, Constants.ApartmentPics);
                            if (!PicsPaths.success) return BadRequest<AddApartmentResponse>("Faild To Save images");
                            await _images.AddImage(PicsPaths.message, apart.Id, PicsPaths.name);
                        }
                    }
                    // Cover!
                    if (request.CoverImage != null)
                    {
                        var CoverImage = await _media.SavingImage(request.CoverImage, request.RequestScheme, request.Requesthost, Constants.ApartmentPics);
                        if (!CoverImage.success) return BadRequest<AddApartmentResponse>("Faild in saving Cover Image");
                        apart.CoverImageName = CoverImage.name;
                        apart.CoverImageUrl = CoverImage.message;
                    }
                }
                if (request.RoyalDocument != null)
                {
                    var RoyalPath = await _media.SavingImage(request.RoyalDocument, request.RequestScheme, request.Requesthost, Constants.ApartmentRoyalDocPics);
                    if (!RoyalPath.success) return BadRequest<AddApartmentResponse>("Faild in saving Cover Image");
                    var x = await _royal.AddRoyal(RoyalPath.message, apart.Id, RoyalPath.name);
                    apart.Document = x.Id;
                }
                await _apartmentServices.UpdateApartmentAsync(apart);
                var returned = new AddApartmentResponse() { id = apart.Id };
                return Success(returned);
            }
            catch (Exception ex)
            {
                return BadRequest<AddApartmentResponse>("Faild");
            }
        }

        public async Task<Response<string>> Handle(AddCommentApartmentCommand request, CancellationToken cancellationToken)
        {
            //validation userid-apartmentid
            var user = await _userManager.FindByIdAsync(request.UserID);
            if (user == null) return BadRequest<string>("No user with this id..!");
            if (!user.EmailConfirmed) return BadRequest<string>("Not Confirmed!");

            var apartment = await _apartmentServices.GetApartment(request.ApartmentID);
            if (apartment == null) return BadRequest<string>("No Apartment with this id..!");
            //mapping
            var comment = new UserApartmentsComment { ApartmentId = request.ApartmentID, Comment = request.Comment, UserId = request.UserID };
            //adding
            //check abilite to add?  
            var checkVar = await _comment.CanCommentOrNo(request.UserID, request.ApartmentID, 3);
            if (checkVar)
            {
                // Add To Notification Table..
                var result = await _comment.AddCommentAsync(comment);
                //return
                if (result == "Success") return Success("");
                return BadRequest<string>("");
            }
            return BadRequest<string>("Can't Comment,Your Limit Comments is Only 3.");

        }

        public async Task<Response<string>> Handle(AddReactApartmentCommand request, CancellationToken cancellationToken)
        {
            //validation userid-apartmentid
            var user = await _userManager.FindByIdAsync(request.UserID);
            if (user == null) return BadRequest<string>("No user with this id..!");
            if (!user.EmailConfirmed) return BadRequest<string>("Not Confirmed!");

            var apartment = await _apartmentServices.GetApartment(request.ApartmentID);
            if (apartment == null) return BadRequest<string>("No Apartment with this id..!");
            //mapping
            var react = new UserApartmentsReact { ApartmentId = request.ApartmentID, UserId = request.UserID };
            //adding
            //check abilite to add?  
            var checkVar = await _react.CanReactOrNo(request.UserID, request.ApartmentID);
            if (checkVar)
            { // Add To Notification Table..
                var result = await _react.AddReactAsync(react);
                if (result == "Success")
                {
                    apartment.Likes++;
                    await _apartmentServices.UpdateApartmentAsync(apartment);
                    return Success("");
                }
                return BadRequest<string>("");
            }
            return BadRequest<string>("Can't React,You Already reacted.");


        }

        public async Task<Response<string>> Handle(PendingApartmentAction request, CancellationToken cancellationToken)
        {
            //valid apartment id

            var apartment = await _apartmentServices.GetApartment(request.ApartmentID);
            if (apartment == null) return BadRequest<string>("No Apartment with this id! ");
            //do operation in service + Delete images if deleted!
            await _apartmentServices.HandlePendingApartments(request.Accept, apartment);
            return Success("");
        }

        public async Task<Response<string>> Handle(DeleteApartmentCommand request, CancellationToken cancellationToken)
        {
            //valid id?
            var apartment = await _apartmentServices.GetApartment(request.ApartmentId);
            if (apartment == null) return NotFound<string>("No Apartmentd With This ID!");
            if (apartment.OwnerId != request.userID) return BadRequest<string>("apartment not belong to this user!!");
            //Any Studnets?
            bool ExistStudent = _usersApartments.AnyStudnets(request.ApartmentId);
            if (ExistStudent) return BadRequest<string>("Exist Students!!");
            //Delete Files
            await _apartmentServices.DeleteApartmentFilesOnly(apartment);

            await _apartmentServices.DeleteApartmentAsync(apartment);
            //return Response
            return Deleted<string>("");
        }

        public async Task<Response<string>> Handle(EditApartmentCommand request, CancellationToken cancellationToken)
        {
            //mapping => Auto mapper!
            var apaa = new Apartment()
            {
                Address = request.Address,
                Description = request.Description,
                Name = request.Title,
                NumberOfUsers = request.numberOfUsers,
                Price = request.price,
                Id = request.ApartmentId
            };
            //Service
            var result = await _apartmentServices.EditApartment(apaa, request.CoverImage, request.Video, request.Pics, request.RequestScheme, request.Requesthost);
            if (result == "Success") return Success("");
            return BadRequest<string>(result);
        }
        #endregion



    }
}
