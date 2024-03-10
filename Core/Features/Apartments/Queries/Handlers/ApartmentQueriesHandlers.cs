using Core.Bases;
using Core.Features.Apartments.Queries.Models;
using Core.Features.Apartments.Queries.Results;
using Domin.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Services.Abstracts;


namespace Core.Features.Apartments.Queries.Handlers
{
    public class ApartmentQueriesHandlers : ResponseHandler, IRequestHandler<GetNotificationApartmentQuery, Response<NotificationApartmentResponse>>
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IViewServices _viewServices;
        private readonly IApartmentServices _apartmentServices;
        private readonly IReactServices _reactServices;
        private readonly ICommentServices _commentServices;
        #endregion

        #region Ctor
        public ApartmentQueriesHandlers(UserManager<ApplicationUser> userManager, IViewServices viewServices, IApartmentServices apartmentServices
                 , IReactServices reactServices, ICommentServices commentServices)
        {
            _userManager = userManager;
            _viewServices = viewServices;
            _apartmentServices = apartmentServices;
            _reactServices = reactServices;
            _commentServices = commentServices;
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
            var userApartments = _apartmentServices.GetUserApartments(request.UserId);
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



        #region Serivces
        private async Task<List<ReturnedNotify>> HandleMapping(List<UserApartmentsComment> comments, List<UserApartmentsReact> reacts, List<View> views)
        {
            var result = new List<ReturnedNotify>();

            //Validate Count
            if (comments.Count() > 0)
            {
                foreach (var comment in comments)
                {
                    var user = await _userManager.FindByIdAsync(comment.UserId);
                    string imageUser = user.imagePath;
                    string imageName = user.imageName;
                    string paths = $"{imageUser}/{imageName}";
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
                    string imageUser = user.imagePath;
                    string imageName = user.imageName;
                    string paths = $"{imageUser}/{imageName}";
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
                    string imageUser = user.imagePath;
                    string imageName = user.imageName;
                    string paths = $"{imageUser}/{imageName}";
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


        private void GetProfileImage(string path)
        {
            // if (System.IO.File.Exists(path))
            //{
            //    // Read the file into a byte array
            //    byte[] imageBytes = System.IO.File.ReadAllBytes(path);

            //    // Return the image as a file response
            //    return File(imageBytes, "image/jpeg", "myImage.jpg");
            //}
            //else
            //{
            //    return NotFound("Image not found");
            //}            //byte[] imageData = System.IO.File.ReadAllBytes("path/to/your/image.jpg");
            //var r=File.ReadAllBytes(paths);
            var paths = "path/to/your/image.jpg";

            using (FileStream stream = new FileStream(paths, FileMode.Open, FileAccess.Read))
            {
                IFormFile file = new FormFile
                    (stream, 0, stream.Length, "fileName", Path.GetFileName(paths));
            }
        }

        #endregion

        #endregion



    }
}

