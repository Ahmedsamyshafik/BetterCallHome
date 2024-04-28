using AutoMapper;
using Core.Bases;
using Core.Features.Users.Commands.Models;
using Core.Features.Users.Commands.Models.ApartmentsRquests;
using Core.Features.Users.Commands.Models.PaymentRequest;
using Core.Features.Users.Commands.Results;
using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.DTO.Payment;
using MediatR;
using Services.Abstracts;

namespace Core.Features.Users.Commands.Handlers
{

    public class UserCommandHandler : ResponseHandler,
                    IRequestHandler<RegisterUserCommand, Response<UserResponse>>,
                    IRequestHandler<LoginUserCommand, Response<UserResponse>>,
                    IRequestHandler<ForgetPasswordUserCommand, Response<string>>,
                    IRequestHandler<ConfirmForgetPasswordUserCommand, Response<string>>,
                    IRequestHandler<EditProfileUserCommand, Response<string>>,
                    IRequestHandler<ResetPasswordUserCommand, Response<string>>,
                    IRequestHandler<ChangePasswordUserCommand, Response<string>>,
                    IRequestHandler<DeleteUserCommand, Response<string>>,
                    IRequestHandler<RequestApartmentStudentCommand, Response<string>>,
                    IRequestHandler<ActionsRequestedApartmentStudentCommand, Response<string>>,
                    IRequestHandler<FormPaymentCommand, Response<string>>
    {

        #region Inject

        private readonly IAuthService _auth;
        private readonly IMapper _mapper;
        private readonly IUserApartmentsRequestsService _requestsService;
        private readonly IApartmentServices _apartmentServices;
        private readonly IUsersApartmentsServices _usersApartmentsServices;
        private readonly IPaymentService _paymentService;



        public UserCommandHandler(IAuthService auth, IMapper mapper, IUserApartmentsRequestsService requestsService,
            IApartmentServices apartmentServices, IUsersApartmentsServices usersApartmentsServices, IPaymentService paymentService)
        {
            _auth = auth;
            _mapper = mapper;
            _requestsService = requestsService;
            _apartmentServices = apartmentServices;
            _usersApartmentsServices = usersApartmentsServices;
            _paymentService = paymentService;
        }
        #endregion


        #region Handle Function

        public async Task<Response<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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
            // response.Role = await _auth.GetUserMaxRole(response.userId);
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
            string result = await _auth.UpdateProfile(mapper, request.Picture, request.RequestScheme, request.Requesthost);
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

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {

            //Service?
            var result = await _auth.DeleteUser(request.UserId);
            //check id? not found
            if (result == "not found") return NotFound<string>("No User with this Id");
            if (result == "Not Confirmed!") return NotFound<string>("Not Confirmed!");
            // Owner With Students in apartment?
            if (result == "Stduent") return NotFound<string>("Thier Are Students with This owner!");
            return Success(result);
        }

        public async Task<Response<string>> Handle(RequestApartmentStudentCommand request, CancellationToken cancellationToken)
        {
            //check user , apartment
            var Existe = await _auth.UserAndApartmentISExist(request.UserID, request.ApartmentID);
            if (Existe == null) return NotFound<string>("No user Or Apartment with this id!");
            //(Check if Exist with other Apartment)
            var isExist = _requestsService.CheckStudentRequestOtherApartment(request.UserID, request.ApartmentID);
            if (isExist) return BadRequest<string>("Can't Enroll in two apartments in same time..!");
            //add to table for waiting requests 
            var paramter = _mapper.Map<UserApartmentsRequests>(request);
            var apartment = await _apartmentServices.GetApartment(request.ApartmentID);
            paramter.OwnerID = apartment.OwnerId;
            //Send To Table?
            await _requestsService.Add(paramter);
            //return response
            return Success("");

        }

        public async Task<Response<string>> Handle(ActionsRequestedApartmentStudentCommand request, CancellationToken cancellationToken)
        {

            // Service to add studnet to apartment or delete this record (requestService) //steps!
            var rec = _requestsService.GetRecord(request.id); //GET Record in (StudentRequest)
            //Add To Apartment if request= true?
            if (request.Accept)
            {
                //auth  
                await _usersApartmentsServices.AddAsync(rec.UserID, rec.ApartmentID);
                //apartment
                await _apartmentServices.AssingStudnetsToApartment(rec.ApartmentID); // As Count
            }

            await _requestsService.DeleteRecord(request.id); //counter of assiging and do it forloop
            //return response
            return Success("");
        }

        public async Task<Response<string>> Handle(FormPaymentCommand request, CancellationToken cancellationToken)
        {
            // mapping
            var paramter = _mapper.Map<FormViewModel>(request);
            //user Property
            var user = await _auth.GetUserById(request.UserID);
            paramter.first_name = user.UserName;
            paramter.last_name = user.UserName;
            paramter.phone_number = user.PhoneNumber;
            paramter.email = user.Email;
            //Service
            var response = await _paymentService.Index(paramter);
            //return
            if (response == "Faild") { return BadRequest<string>("Faild"); }
            return Success(response);
        }


        #endregion

    }
}
