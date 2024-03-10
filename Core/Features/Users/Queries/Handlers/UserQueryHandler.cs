using AutoMapper;
using Core.Bases;
using Core.Features.Users.Queries.Models;
using Core.Features.Users.Queries.Results;
using Domin.Models;
using MediatR;
using Services.Abstracts;

namespace Core.Features.Users.Queries.Handlers
{
    public class UserCommandHandler : ResponseHandler,
        IRequestHandler<GetProfileDataQuery, Response<UserDataResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IViewServices _view;
        private readonly IMapper _mapper;
        public UserCommandHandler(IAuthService authService, IViewServices view, IMapper mapper)
        {
            _authService = authService;
            _view = view;
            _mapper = mapper;
        }
        public async Task<Response<UserDataResponse>> Handle(GetProfileDataQuery request, CancellationToken cancellationToken)
        {
            //Get user from httpContext {id}
            if (request.RequesterUserID == null) return BadRequest<UserDataResponse>("Only Authorize people");
            //valid id & (get data & Counter++)
            var data = await _authService.GetUserData(request.RequesterUserID,request.UserId);
            if (data.Message != "Success") { return BadRequest<UserDataResponse>($"{data.Message}"); }
            //increase views Counter in ViwedUser (requester , requested ,Counter ++ , id ++)
            var addView = new View { count = data.OperationCount, ViewerID = request.RequesterUserID, ViewedID = data.user.Id };
            await _view.AddView(addView);
            //adding in notification table
            //map data from applicationUser to Response
            var result = _mapper.Map<UserDataResponse>(data.user);
            //return data 
            return Success(result);
        }
    }
}

