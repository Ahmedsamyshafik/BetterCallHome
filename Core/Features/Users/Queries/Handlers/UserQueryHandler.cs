using AutoMapper;
using Core.Bases;
using Core.Features.Users.Queries.Models;
using Core.Features.Users.Queries.Results;
using Core.Wrappers;
using Domin.Constant;
using Domin.Models;
using Infrastructure.DTO;
using MediatR;
using Services.Abstracts;

namespace Core.Features.Users.Queries.Handlers
{
    public class UserCommandHandler : ResponseHandler,
        IRequestHandler<GetProfileDataQuery, Response<UserDataResponse>>,
        IRequestHandler<GetAllUserQuery, PaginatedResult<GetAllUsersResponse>>
    {
        #region Inject
        private readonly IAuthService _authService;
        private readonly IViewServices _view;
        private readonly IMapper _mapper;
        private readonly IApartmentServices _apartmentServices;
        public UserCommandHandler(IAuthService authService, IViewServices view, IMapper mapper, IApartmentServices apartmentServices)
        {
            _authService = authService;
            _view = view;
            _mapper = mapper;
            _apartmentServices = apartmentServices;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<UserDataResponse>> Handle(GetProfileDataQuery request, CancellationToken cancellationToken)
        {
            //Get user from httpContext {id}
            if (request.RequesterUserID == null) return BadRequest<UserDataResponse>("Only Authorize people");
            //valid id & (get data & Counter++)
            var data = await _authService.GetUserData(request.RequesterUserID, request.RequestedId);
            if (data.Message != "Success") { return BadRequest<UserDataResponse>($"{data.Message}"); }
            //increase views Counter in ViwedUser (requester , requested ,Counter ++ , id ++)
            var addView = new View { count = data.OperationCount, ViewerID = request.RequesterUserID, ViewedID = data.user.Id };
            await _view.AddView(addView);
            //adding in notification table
            //map data from applicationUser to Response
            var result = _mapper.Map<UserDataResponse>(data.user);
            result.Views = data.OperationCount;
            //Apartments?
            result.Roles = await _authService.GetUserMaxRole(data.user.Id);
            if (result.Roles.Contains(Constants.OwnerRole))
            {
                //Service to get Number of owner apartments
                result.Apartments = _apartmentServices.GetOwnerApartments(result.UserID).Count();
            }
            //return data 
            return Success(result);
        }

        public async Task<PaginatedResult<GetAllUsersResponse>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {

            var list = await _authService.GetAllUsers(request.Search);
            var x = list.AsQueryable();
            var paginatin = await x.ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatin;
        }
        #endregion

    }
}

