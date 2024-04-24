using AutoMapper;
using Core.Bases;
using Core.Features.Apartments.Queries.Models;
using Core.Features.Apartments.Queries.Results;
using Core.Wrappers;
using Domin.Models;
using infrustructure.DTO.Apartments.Pagination;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;
using static Services.Implementations.UsersApartmentsServices;


namespace Core.Features.Apartments.Queries.Handlers
{
    public class ApartmentQueriesHandlers : ResponseHandler,
        IRequestHandler<GetNotificationApartmentQuery, Response<NotificationApartmentResponse>>,
        IRequestHandler<GetPendingApartmentsQuery, PaginatedResult<GetPendingApartmentsPaginationResponse>>,
        IRequestHandler<GetApartmentsForOwnerQuery, PaginatedResult<GetPendingApartmentsForOwnerPaginationResponse>>,
        IRequestHandler<GetApartmentPagintation, PaginatedResult<GetApartmentPagintationResponse>>,
        IRequestHandler<GetApartmentDetailQuery, Response<GetApartmentDetailResponse>>,
        IRequestHandler<GetApartmentForEditQuery, Response<GetApartmentForEditResponse>>,
        IRequestHandler<GetApartmentLandingPageQuery, Response<List<GetApartmentLandingPageResponse>>>
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IViewServices _viewServices;
        private readonly IApartmentServices _apartmentServices;
        private readonly IReactServices _reactServices;
        private readonly ICommentServices _commentServices;
        private readonly IMapper _mapper;
        private readonly IUsersApartmentsServices _usersApartments;
        private readonly IUserApartmentsRequestsService _usersApartmentsRequests;
        private readonly IImagesServices _images;
        private readonly IVideosServices _videos;
        #endregion

        #region Ctor
        public ApartmentQueriesHandlers(UserManager<ApplicationUser> userManager, IViewServices viewServices, IApartmentServices apartmentServices
                 , IReactServices reactServices, ICommentServices commentServices, IMapper mapper, IUsersApartmentsServices usersApartments,
            IImagesServices images, IUserApartmentsRequestsService usersApartmentsRequests, IVideosServices videos)
        {
            _userManager = userManager;
            _viewServices = viewServices;
            _apartmentServices = apartmentServices;
            _reactServices = reactServices;
            _commentServices = commentServices;
            _mapper = mapper;
            _usersApartments = usersApartments;
            _images = images;
            _usersApartmentsRequests = usersApartmentsRequests;
            _videos = videos;
        }
        #endregion

        #region Handle Functions

        public async Task<Response<NotificationApartmentResponse>> Handle(GetNotificationApartmentQuery request, CancellationToken cancellationToken)
        {
            //valid userid
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return BadRequest<NotificationApartmentResponse>();
            if (!user.EmailConfirmed) return BadRequest<NotificationApartmentResponse>("Not Confirmed!");
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

            if (apartmentIds.Count > 0) { reacts = await _reactServices.GetApartmentsReacts(apartmentIds); }
            //---get Comments (Same Like Reacts)---
            List<UserApartmentsComment> comments = new();
            if (apartmentIds.Count > 0) { comments = await _commentServices.GetApartmentsComments(apartmentIds); }
            //Get Requests 
            var OwnerRequests = _usersApartmentsRequests.GetOwnerRequests(request.UserId);
            //Get Response?
            var StudentResponse = await _usersApartments.GetNotyForAcceptStudentAsync(request.UserId);//check IsExist
            if (StudentResponse.isEixst)
            {
                //ApartmentID?=> Apartment=>Owner
                var owner = await _apartmentServices.GetApartment(StudentResponse.apartmentId);
                StudentResponse.OwnerID = owner.OwnerId;
            }
            //return Resoonse!
            var response = new NotificationApartmentResponse();
            response.notifies = await HandleMapping(comments, reacts, userViews, OwnerRequests, StudentResponse);//Requests,Responses
            return Success(response);

        }

        public async Task<PaginatedResult<GetPendingApartmentsPaginationResponse>> Handle(GetPendingApartmentsQuery request, CancellationToken cancellationToken)
        {
            var test = _apartmentServices.getPendingpaginate(request.Search);

            var paginationList = await _mapper.ProjectTo<GetPendingApartmentsPaginationResponse>(test)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return paginationList;
        }

        public async Task<PaginatedResult<GetPendingApartmentsForOwnerPaginationResponse>> Handle(GetApartmentsForOwnerQuery request, CancellationToken cancellationToken)
        {
            var test = _apartmentServices.getpaginateForOwner(request.OwnerId, request.Search);
            test.OrderBy(x => x.PublishedAt);
            

            var paginationList = await _mapper.ProjectTo<GetPendingApartmentsForOwnerPaginationResponse>(test)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return paginationList;
        }

        public async Task<PaginatedResult<GetApartmentPagintationResponse>> Handle(GetApartmentPagintation request, CancellationToken cancellationToken)
        {
            //Get All Apartments in Pagination
            var test = _apartmentServices.getApartmentspaginate(request.Search, request.City, request.Gender, request.CountInApartment, request.minPrice, request.maxPrice);
            test.OrderByDescending(x => x.PublishedAt);
            //from  x=>x!!
            var paginationList = await _mapper.ProjectTo<GetApartmentPagintationResponse>(test)
              .ToPaginatedListAsync(request.PageNumber, request.PageSize);


            return paginationList;
        }

        public async Task<Response<GetApartmentDetailResponse>> Handle(GetApartmentDetailQuery request, CancellationToken cancellationToken)
        {
            var lsta = await _apartmentServices.GetApartmentDetails(request.Id);
            var x = lsta.ApartmentComments.AsQueryable();
            //mapping?
            var paginationComments = await _mapper.ProjectTo<ApartmentCommentsResponse>(x)
              .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            //mapping to response
            var response = _mapper.Map<GetApartmentDetailResponse>(lsta);
            response.ApartmentComments = paginationComments;
            return Success(response);
        }

        public async Task<Response<GetApartmentForEditResponse>> Handle(GetApartmentForEditQuery request, CancellationToken cancellationToken)
        {
            //Get Apartment
            var apartment = await _apartmentServices.GetApartment(request.ApartmentId);
            if (apartment == null) return NotFound<GetApartmentForEditResponse>();
            //Mapping
            //Apartment=>GetApartmentForEditRespons
            var response = _mapper.Map<GetApartmentForEditResponse>(apartment);
            //Assing Pics
            var images = _images.GetApartmentImgs(apartment.Id);
            List<string> urls = new();
            foreach (var image in images)
            {
                urls.Add(image.ImageUrl);
            }
            response.ApartmentPics = urls;
            //Asing Video ?
            var Vid = _videos.GetApartmentdVideo(apartment.Id);
            response.ApartmentVideo = Vid.FirstOrDefault();
            //Return
            return Success(response);
        }

        public async Task<Response<List<GetApartmentLandingPageResponse>>> Handle(GetApartmentLandingPageQuery request, CancellationToken cancellationToken)
        {
            // Get First 3 Apartment According To Likes
            var apartments = _apartmentServices.GetApartmentsTopRateForLandingPage();
            //mapping 
            var response = _mapper.Map<List<GetApartmentLandingPageResponse>>(apartments);
            //return
            return Success(response);
        }


        #endregion

        #region Private Fucntions
        private async Task<List<ReturnedNotify>> HandleMapping(List<UserApartmentsComment> comments, List<UserApartmentsReact> reacts, List<View> views
            , List<UserApartmentsRequests> OwnerRequests, ReturnedNotifyForStudent StudentResponse)
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

            if (OwnerRequests.Count() > 0)
            {
                foreach (var request in OwnerRequests)
                {
                    var user = await _userManager.FindByIdAsync(request.UserID);

                    string paths = user.imageUrl;
                    //Service To Upload Image By Path + imgae Name.(check)
                    var RequestMapping = new ReturnedNotify
                    {
                        Type = "OwnerRequest",
                        OperationId = request.Id,
                        UserName = user.UserName,
                        date = request.DateTime,
                        UserImage = paths
                    };
                    result.Add(RequestMapping);
                }
            }

            if (StudentResponse.isEixst == true)
            {
                var owner = await _userManager.FindByIdAsync(StudentResponse.OwnerID);
                var RequestMapping = new ReturnedNotify
                {
                    Type = "Accept",
                    OperationId = StudentResponse.OperationId,
                    UserName = owner.UserName,//Owner Name !
                    date = StudentResponse.date,
                    UserImage = owner.imageUrl
                };
                result.Add(RequestMapping);


            }

            return result;
        }

        #endregion
    }
}

