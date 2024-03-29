﻿using AutoMapper;
using Core.Bases;
using Core.Features.Users.Commands.Models;
using Core.Features.Users.Commands.Results;
using Domin.Models;
using Infrastructure.DTO;
using MediatR;
using Services.Abstracts;

namespace Core.Features.Users.Commands.Handlers
{

    public class UserCommandHandler : ResponseHandler,
                    IRequestHandler<RegisterStudentCommand, Response<UserResponse>>,
                    IRequestHandler<LoginUserCommand, Response<UserResponse>>,
                    IRequestHandler<ForgetPasswordUserCommand, Response<string>>,
                    IRequestHandler<ConfirmForgetPasswordUserCommand, Response<string>>,
                    IRequestHandler<EditProfileUserCommand, Response<string>>,
                    IRequestHandler<ResetPasswordUserCommand, Response<string>>,
                    IRequestHandler<ChangePasswordUserCommand, Response<string>>
    {

        #region Inject

        private readonly IAuthService _auth;
        private readonly IMapper _mapper;


        public UserCommandHandler(IAuthService auth, IMapper mapper)
        {
            _auth = auth;
            _mapper = mapper;

        }
        #endregion


        #region Handle Function

        public async Task<Response<UserResponse>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
        {
            // (mapping)  RegisterUserCommand => RegisterDTO
            var paramater = _mapper.Map<RegisterDTO>(request);
            // (Service) User=> {UserResponse}
            var result = await _auth.RegisterAsync(paramater);

            //From UserDTO to UserResponse
            var response = _mapper.Map<UserResponse>(result);
            // Service Response
            //if faild
            if (!result.IsAuthenticated) return BadRequest<UserResponse>(Message: response.Message);
            //success?
            return Success(response);
         
        }

        public async Task<Response<UserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            //mapping from loginUserCommand To LoginDTo
         
            var paramater = _mapper.Map<LoginDTO>(request);
            //service To login
            var result = await _auth.Login(paramater);
            //mapping from UserDTO to UserResponse
            var response = _mapper.Map<UserResponse>(result);
            //Service Response
            //if Faild
            if (!result.IsAuthenticated)
            {
                return BadRequest<UserResponse>(result.Message);
            }
            //Success
            return Success(response);
        }

        public async Task<Response<string>> Handle(ForgetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _auth.ForgetPassword(request.Email);
            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            return BadRequest<string>(result.Message);
        }

        public async Task<Response<string>> Handle(ConfirmForgetPasswordUserCommand request, CancellationToken cancellationToken)
        {

            var result = await _auth.ConfirmForgetPassword(request.email, request.code);
            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            return BadRequest<string>(result.Message);
        }

        public async Task<Response<string>> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            // ResetPassword(string email, string Password, string ConfrimPassword);
            var result = await _auth.ResetPassword(request.Email, request.NewPassword, request.ConfirmPassword);
            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            return BadRequest<string>(result.Message);

        }

        public async Task<Response<string>> Handle(EditProfileUserCommand request, CancellationToken cancellationToken)
        {
            //map to Application User
            var mapper = _mapper.Map<ApplicationUser>(request);  // password? oldPassword!! Img
            //Service To Update {send image package..}
            string result = await _auth.UpdateStudentandOwnerProfile(mapper, request.Picture,request.RequestScheme,request.Requesthost);
            //testing for image
            //bool res = await _image.UploadAsync(request.Picture);
            //check service response 
            if (result == "Success") return Success("");
            return BadRequest<string>(result);
        }

        public async Task<Response<string>> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _auth.ChangePassword(request.Email, request.Password, request.NewPassword);
            if (!result.IsSuccess)
            {
                return BadRequest<string>(result.Message);
            }
            return Success("");
        }


        #endregion

    }
}
