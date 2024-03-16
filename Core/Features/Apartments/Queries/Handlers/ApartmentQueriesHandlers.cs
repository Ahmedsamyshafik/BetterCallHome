using AutoMapper;
using Core.Bases;
using Core.Features.Apartments.Queries.Models;
using Core.Features.Apartments.Queries.Results;
using Domin.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;


namespace Core.Features.Apartments.Queries.Handlers
{
    public class ApartmentQueriesHandlers : ResponseHandler,
        IRequestHandler<GetNotificationApartmentQuery, Response<NotificationApartmentResponse>>,
        IRequestHandler<GetPendingApartmentsQuery, Response<PendingApartmentsResponse>>
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IViewServices _viewServices;
        private readonly IApartmentServices _apartmentServices;
        private readonly IReactServices _reactServices;
        private readonly ICommentServices _commentServices;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public ApartmentQueriesHandlers(UserManager<ApplicationUser> userManager, IViewServices viewServices, IApartmentServices apartmentServices
                 , IReactServices reactServices, ICommentServices commentServices, IMapper mapper)
        {
            _userManager = userManager;
            _viewServices = viewServices;
            _apartmentServices = apartmentServices;
            _reactServices = reactServices;
            _commentServices = commentServices;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions

        public async Task<Response<NotificationApartmentResponse>> Handle(GetNotificationApartmentQuery request, CancellationToken cancellationToken)
        {
            //valid userid
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return BadRequest<NotificationApartmentResponse>();
            //get views
            var userViews = _viewServices.GetAccountViews(request.UserId);
            //---get likes---
            // user apartments
            var userApartments = _apartmentServices.GetOwnerApartments(request.UserId);
            //get likes by apartment_id
            List<int> apartmentIds = new List<int>();
            foreach (var apartmentId in userApartments)
            {
                apartmentIds.Add(apartmentId.Id);
            }
            List<UserApartmentsReact> reacts = new();

            if (apartmentIds.Count > 0) { reacts = await _reactServices.GetApartmentReacts(apartmentIds); }
            //---get Comments (Same Like Reacts)---
            List<UserApartmentsComment> comments = new();
            if (apartmentIds.Count > 0) { comments = await _commentServices.GetApartmentComments(apartmentIds); }

            //return Resoonse!
            var response = new NotificationApartmentResponse();
            response.notifies = await HandleMapping(comments, reacts, userViews);
            return Success(response);

        }

        public async Task<Response<PendingApartmentsResponse>> Handle(GetPendingApartmentsQuery request, CancellationToken cancellationToken)
        {
            //Get Apartments (Publish=Flase)
            var apartments = await _apartmentServices.GetPendingApartmentd();
            //Get Apartment Owner
            List<string> ownersId = new();
            foreach (var ownerId in apartments)
            {
                ownersId.Add(ownerId.OwnerId);
            }
            List<ApplicationUser> owners = new();
            foreach (var userID in ownersId)
            {
                owners.Add(await _userManager.FindByIdAsync(userID));
            }
            //mapp owner and apartment together 
            List<TempMappingApartmentsOwnersToPendint> temp = new();
            for (int i = 0; i < apartments.Count(); i++)
            {
                TempMappingApartmentsOwnersToPendint one = new()
                {
                    Apartment = apartments[i],
                    user = owners[i]
                };
                temp.Add(one);
            }
            var result = new List<PendingCollection>();
            foreach (var res in temp)
            {
                var x = _mapper.Map<PendingCollection>(res);
                result.Add(x);
            }
            var response = new PendingApartmentsResponse() { PendingApartments = result };
            return Success(response);
            //Mapping Data
            //map from TempMappingApartmentOwnersToPendint To PendingApartmentsResponse
            //   var response = _mapper.Map<List<PendingApartmentsResponse>>(temp);
            //Return
        }

        private async Task<List<ReturnedNotify>> HandleMapping(List<UserApartmentsComment> comments, List<UserApartmentsReact> reacts, List<View> views)
        {
            var result = new List<ReturnedNotify>();

            //Validate Count
            if (comments.Count() > 0)
            {
                foreach (var comment in comments)
                {
                    var user = await _userManager.FindByIdAsync(comment.UserId);

                    string paths = user.imageUrl;
                    //Service To Upload Image By Path + imgae Name.(check)
                    var commentMapping = new ReturnedNotify
                    {
                        Type = "comment",
                        OperationId = comment.Id,
                        UserName = user.UserName,
                        date = comment.Date,
                        UserImage = paths
                    };
                    result.Add(commentMapping);
                }
            }

            if (reacts.Count() > 0)
            {
                foreach (var react in reacts)
                {
                    var user = await _userManager.FindByIdAsync(react.UserId);

                    string paths = user.imageUrl;
                    //Service To Upload Image By Path + imgae Name.(check)
                    var reactMapping = new ReturnedNotify
                    {
                        Type = "react",
                        OperationId = react.Id,
                        UserName = user.UserName,
                        date = react.Date,
                        UserImage = paths
                    };
                    result.Add(reactMapping);
                }
            }

            if (views.Count() > 0)
            {
                foreach (var view in views)
                {
                    var user = await _userManager.FindByIdAsync(view.ViewerID);

                    string paths = user.imageUrl;
                    //Service To Upload Image By Path + imgae Name.(check)
                    var viewMapping = new ReturnedNotify
                    {
                        Type = "view",
                        OperationId = view.Id,
                        UserName = user.UserName,
                        date = view.DateTime,
                        UserImage = paths
                    };
                    result.Add(viewMapping);
                }
            }

            return result;
        }

        #endregion
    }
}

