﻿using AutoMapper;
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
                    IRequestHandler<AddApartmentCommand, Response<string>>,
                    IRequestHandler<AddCommentApartmentCommand, Response<string>>,
                    IRequestHandler<AddReactApartmentCommand, Response<string>>
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
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion
        #region Ctor
        public ApartmentCommandHandler(IApartmentServices apartmentServices, IMapper mapper, IUploadingMedia media
                    , IVideosServices videos, IImagesServices images, IRoyalServices royal, UserManager<ApplicationUser> userManager
                   , ICommentServices comment, IReactServices react)
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
        }
        #endregion


        #region MyRegion

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
                    var videoPath = await _media.SavingImage(request.video, request.RequestScheme, request.Requesthost, Constants.ApartmentVids);
                    if (!videoPath.success) return BadRequest<string>("Faild in saving video");
                    var x = await _videos.AddVideo(videoPath.message, apart.Id);
                    apart.ApartmentVideoID = x.Id;
                }
                if (request.Pics != null)
                {

                    foreach (var Pic in request.Pics)
                    {
                        if (Pic.Length > 0)
                        {
                            var PicsPaths = await _media.SavingImage(Pic, request.RequestScheme, request.Requesthost, Constants.ApartmentPics);
                            if (!PicsPaths.success) return BadRequest<string>("Faild To Save images");
                            await _images.AddImage(PicsPaths.message, apart.Id);
                        }
                    }
                    // Cover!
                    if (request.CoverImage != null)
                    {
                        var CoverImage = await _media.SavingImage(request.CoverImage, request.RequestScheme, request.Requesthost, Constants.ApartmentPics);
                        if (!CoverImage.success) return BadRequest<string>("Faild in saving Cover Image");
                        apart.CoverImageName = CoverImage.message;
                    }
                }
                if (request.RoyalDocument != null)
                {
                    var RoyalPath = await _media.SavingImage(request.RoyalDocument, request.RequestScheme, request.Requesthost, Constants.ApartmentRoyalDocPics);
                    if (!RoyalPath.success) return BadRequest<string>("Faild in saving Cover Image");
                    var x = await _royal.AddRoyal(RoyalPath.message, apart.Id);
                    apart.Document = x.Id;
                }
                await _apartmentServices.UpdateApartmentAsync(apart);
                return Success("");
            }
            catch (Exception ex)
            {
                var exap = ex;
                return BadRequest<string>("Faild");
            }
        }

        public async Task<Response<string>> Handle(AddCommentApartmentCommand request, CancellationToken cancellationToken)
        {
            //validation userid-apartmentid
            var user = await _userManager.FindByIdAsync(request.UserID);
            if (user == null) return BadRequest<string>("No user with this id..!");
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
        #endregion



    }
}
