using AutoMapper;
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
                    IRequestHandler<LoginUserAdminCommand, Response<UserResponse>>,
                    IRequestHandler<LoginUserOwnerCommand, Response<UserResponse>>,
                    IRequestHandler<ChangePasswordUserCommand, Response<string>>,
                    IRequestHandler<ResetPasswordUserCommand, Response<string>>,
                    IRequestHandler<EditProfileStudentandOwnerCommand, Response<string>>
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

        public async Task<Response<UserResponse>> Handle(LoginUserAdminCommand request, CancellationToken cancellationToken)
        {
            //map from LoginUserAdminCommand to LoginDto
            var paramter = _mapper.Map<LoginDTO>(request);
            //Service
            var result = await _auth.LoginForAdmin(paramter);
            //mapping from UserDto To UserResponse
            var response = _mapper.Map<UserResponse>(result);
            //return BadRequest
            if (!result.IsAuthenticated) { return BadRequest<UserResponse>(result.Message); }
            //Success
            return Success(response);
        }

        public async Task<Response<UserResponse>> Handle(LoginUserOwnerCommand request, CancellationToken cancellationToken)
        {
            //map from LoginUserAdminCommand to LoginDto
            var paramter = _mapper.Map<LoginDTO>(request);
            //Service
            var result = await _auth.LoginForOwners(paramter);
            //mapping from UserDto To UserResponse
            var response = _mapper.Map<UserResponse>(result);
            //return BadRequest
            if (!result.IsAuthenticated) { return BadRequest<UserResponse>(result.Message); }
            //Success
            return Success(response);
        }

        public async Task<Response<string>> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
        {
            //is Success Emailing
            var result = await _auth.ForgetPassword(request.Email);
            if (result.IsSuccess)
            {
                return Success(result.Message);
            }
            return BadRequest<string>(result.Message);
        }

        public async Task<Response<string>> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            //mapping from ResetPasswordUserCommand To ResetPasswordDTO
            var paramter = _mapper.Map<ResetPasswordDTO>(request);
            var result = await _auth.ResetPassword(paramter);
            if (result.IsSuccess)
            {

                return Success(result.Message);
            }
            else
            {
                return BadRequest<string>(result.Message);
            }

        }

        public async Task<Response<string>> Handle(EditProfileStudentandOwnerCommand request, CancellationToken cancellationToken)
        {
            //map to Application User
            var mapper = _mapper.Map<ApplicationUser>(request);  // password? oldPassword!! Img
            //Service To Update 
            string result = await _auth.UpdateStudentandOwnerProfile(mapper, request.Picture);
            //testing for image
            //bool res = await _image.UploadAsync(request.Picture);
            //check service response 
            if (result == "Success") return Success("");
            return BadRequest<string>(result);
        }


        #endregion

    }
}
