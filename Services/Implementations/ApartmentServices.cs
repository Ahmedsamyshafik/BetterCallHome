using Domin.Constant;
using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.DTO.Apartments.Detail;
using Infrastructure.Repository.IRepository;
using infrustructure.DTO.Apartments.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;
using System.Reflection.Metadata;

namespace Services.Implementations
{
    public class ApartmentServices : IApartmentServices
    {
        #region Fields
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IImagesServices _imagesService;
        private readonly IRoyalServices _royalServices;
        private readonly IVideosServices _videosService;
        private readonly IUploadingMedia _media;
        private readonly IUsersApartmentsServices _usersApartmentsServices;
        private readonly IReactServices _reactServices;
        private readonly UserManager<ApplicationUser> _authService;
        private readonly ICommentServices _commentServices;
        #endregion

        #region Ctor
        public ApartmentServices(IApartmentRepository apartmentRepository, IImagesServices imagesService,
            IRoyalServices royalServices, IVideosServices videosService, IUploadingMedia media,
            IUsersApartmentsServices usersApartmentsServices, IReactServices reactServices, UserManager<ApplicationUser> authService
            , ICommentServices commentServices)
        {
            _apartmentRepository = apartmentRepository;
            _imagesService = imagesService;
            _royalServices = royalServices;
            _videosService = videosService;
            _media = media;
            _usersApartmentsServices = usersApartmentsServices;
            _reactServices = reactServices;
            _authService = authService;
            _commentServices = commentServices;
        }
        #endregion

        #region Handle Functions
        public async Task<Apartment> AddApartmentAsync(Apartment apartment)
        {
            return await _apartmentRepository.AddAsync(apartment);

        }

        public async Task<string> UpdateApartmentAsync(Apartment apartment)
        {
            await _apartmentRepository.UpdateAsync(apartment);
            return "";
        }

        public async Task<string> DeleteApartmentFilesOnly(Apartment apartment)
        {
            //  delete imgs , videos , doc
            await _imagesService.DeleteApartmentImageFile(apartment.Id);
            await _videosService.DeleteApartmentVideoFile(apartment.Id);
            await _royalServices.DeleteApartmentDocumentFile(apartment.Id);
            //await _apartmentRepository.DeleteAsync(apartment);
            return "";
        }

        public async Task<string> DeleteApartmentAsync(Apartment apartment)
        {
            await _apartmentRepository.DeleteAsync(apartment);
            return "";
        }

        public async Task<Apartment> GetApartment(int apartmentId)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(apartmentId);
            return apartment;
        }

        public IQueryable<Apartment> GetOwnerApartments(string userID)
        {
            var apartments = _apartmentRepository.GetTableNoTracking().Where(x => x.OwnerId == userID);
            return apartments;
        }

        public async Task<List<Apartment>> GetOwnerApartmentsAsList(string userID)
        {
            var apartments = await _apartmentRepository.GetTableNoTracking().Where(x => x.OwnerId == userID).ToListAsync();
            return apartments;
        }


        public IQueryable<Apartment> GetPendingApartmentd(string? search)
        {
            var x = _apartmentRepository.GetTableNoTracking().Where(x => x.Publish == false);
            if (search != null)
            {
                x = x.Where(x => x.Name.Contains(search) || x.Address.Contains(search));
            }
            return x;
        }


        public IQueryable<ApartmentPaginationPending> getPendingpaginate(string? search)
        {
            var lst = _apartmentRepository.GetTableNoTracking().
                Include(x => x.Owner)
                .Where(x => x.Publish == false);

            var imgs = _imagesService.GetAlll();
            var videos = _videosService.GetAll();
            var royals = _royalServices.GetAll();

            if (search != null)
            {
                lst = lst.Where(x => x.Name.Contains(search));
            }
            List<ApartmentPaginationPending> result = new List<ApartmentPaginationPending>();
            foreach (var apart in lst)
            {
                var temp = new ApartmentPaginationPending();

                temp.Address = apart.Address;
                temp.ApartmentTitle = apart.Name;
                temp.NumberOfUsers = apart.NumberOfUsers;
                //  temp.ApartmentCoverImage = apart.CoverImageName;
                temp.PublishedAt = apart.CreatedAt;
                temp.Salary = apart.Price;
                temp.ApartmentID = apart.Id;

                temp.OwnerName = apart.Owner.UserName;
                temp.OwnerImage = apart.Owner.imageUrl;

                var images = imgs.Where(x => x.ApartmentID == apart.Id).ToList();
                var Vids = videos.Where(x => x.ApartmentID == apart.Id).ToList();
                var Rols = royals.Where(x => x.ApartmentID == apart.Id).ToList();
                List<string> urls = new List<string>();
                urls.Add(apart.CoverImageUrl);

                foreach (var image in images)
                {
                    string url = image.ImageUrl;
                    if (url != null) urls.Add(url);

                }
                foreach (var video in Vids)
                {
                    string url = video.VideoUrl;
                    if (url != null) urls.Add(url);
                }
                foreach (var royal in Rols)
                {
                    string url = royal.ImageUrl;
                    if (url != null) urls.Add(url);
                }
                temp.ApartmentsPics = urls;
                result.Add(temp);
            }
            return result.AsQueryable();
        }

        public IQueryable<GetPendingApartmentsForOwnerPaginationDTO> getpaginateForOwner(string ownerID, string? search)
        {
            var lst = _apartmentRepository.GetTableNoTracking()
                .Where(x => x.OwnerId == ownerID && x.Publish == true);
            if (search != null)
            {
                lst = lst.Where(x => x.Name.Contains(search));
            }
            List<GetPendingApartmentsForOwnerPaginationDTO> result = new List<GetPendingApartmentsForOwnerPaginationDTO>();
            foreach (var apart in lst)
            {
                var temp = new GetPendingApartmentsForOwnerPaginationDTO();
                temp.Address = apart.Address;
                temp.ApartmentTitle = apart.Name;
                temp.NumberOfUsers = apart.NumberOfUsers;
                temp.ApartmentCoverImage = apart.CoverImageName;
                temp.PublishedAt = apart.CreatedAt;
                temp.Salary = apart.Price;
                temp.ApartmentID = apart.Id;
                result.Add(temp);
            }
            return result.AsQueryable();
        }

        public async Task<string> HandlePendingApartments(bool Accept, Apartment apartment)
        {
            if (Accept)
            {
                apartment.Publish = true;
                await UpdateApartmentAsync(apartment);

            }
            else
            {
                //Delete? img first?(location?)
                //imageApartment
                await _imagesService.DeleteApartmentImageFile(apartment.Id);
                //Apartment Videos
                await _videosService.DeleteApartmentVideoFile(apartment.Id);
                //Apartment Document
                await _royalServices.DeleteApartmentDocumentFile(apartment.Id);
                //Deleting
                await DeleteApartmentAsync(apartment);
                //see?

            }
            return "";
        }

        public async Task<string> EditApartment(Apartment apartment, IFormFile? CoverImage, IFormFile? Video, List<IFormFile>? Pics,
            string requestSchema, HostString host)
        {
            var DBApartment = await _apartmentRepository.GetByIdAsync(apartment.Id);
            DBApartment.Address = apartment.Address;
            DBApartment.Description = apartment.Description;
            DBApartment.Price = apartment.Price;
            DBApartment.Name = apartment.Name;
            DBApartment.NumberOfUsers = apartment.NumberOfUsers;
            //images-videos
            //CoverImage
            if (CoverImage != null)
            {
                //Delete Old
                await _imagesService.DeleteApartmentCoverImage(apartment.Id);
                //Save New
                var x = await _media.SavingImage(CoverImage, requestSchema, host, Constants.ApartmentPics);
                //messeag=>url & name=>name
                if (x.success)
                {
                    DBApartment.CoverImageUrl = x.message;
                    DBApartment.CoverImageName = x.name;
                }
                else
                {
                    return "Faild in cove image";
                }

            }
            if (Pics != null)
            {
                //Delete Old
                await _imagesService.DeleteApartmentPics(apartment.Id);
                //Saving New 
                foreach (var p in Pics)
                {
                    //Save New
                    var x = await _media.SavingImage(p, requestSchema, host, Constants.ApartmentPics);
                    if (x.success) await _imagesService.AddImage(x.message, apartment.Id, x.name);
                    else
                    {
                        return "Faild in  images";
                    }
                }

            }
            if (Video != null)
            {   //Delete old (File)
                await _videosService.DeleteApartmentVideoFile(apartment.Id);
                //Delete Old (DB)
                await _videosService.DeleteVideo(apartment.Id);
                //Save new
                var videoPath = await _media.SavingImage(Video, requestSchema, host, Constants.ApartmentVids);
                if (videoPath.success)
                {
                    var x = await _videosService.AddVideo(videoPath.message, apartment.Id, videoPath.name);

                    DBApartment.ApartmentVideoID = x.Id;
                }
                else
                {
                    return "Faild in  Video";
                }

            }
            return "Success";
        }

        #region Housing
        public async Task<string> AssingStudnetsToApartment(int apartmentId)
        {
            var apartment = await _apartmentRepository.GetByIdAsync(apartmentId);
            apartment.NumberOfUsersExisting++;
            await _apartmentRepository.UpdateAsync(apartment);
            return "Success";
        }

        public IQueryable<GetApartmentPagintationResponse> getApartmentspaginate(string? search, string? city, string? gender,
         int countIn, decimal? min, decimal? max)
        {
            // Filteration!
            var lst = _apartmentRepository.GetTableNoTracking().Where(x => x.Publish == true).ToList();
            if (search != null) lst = lst.Where(x => x.Name.Contains(search)).ToList();
            if (city != null) lst = lst.Where(x => x.City.Equals(city)).ToList();
            if (gender != null) lst = lst.Where(x => x.gender.Equals(gender)).ToList();
            if (countIn > 0) lst = lst.Where(x => x.NumberOfUsers >= countIn).ToList();
            if (min > 0) lst = lst.Where(x => x.Price >= min).ToList();
            if (max > 0) lst = lst.Where(x => x.Price <= min).ToList();

            List<GetApartmentPagintationResponse> result = new();
            foreach (var apart in lst)
            {
                var temp = new GetApartmentPagintationResponse();

                temp.Address = apart.Address;
                temp.Name = apart.Name;
                temp.TotalCount = apart.NumberOfUsers; // Apartment Total Count
                //Student Already in ! => UserApartments
                temp.ApartmentExistCount = _usersApartmentsServices.GetCountStudentsInApartment(apart.Id);
                temp.PublishedAt = (DateTime)apart.CreatedAt;
                temp.Salary = apart.Price;
                temp.ApartmentID = apart.Id;
                temp.ImageURL = apart.CoverImageUrl;
                temp.City = apart.City;
                result.Add(temp);
            }
            return result.AsQueryable();
        }

        public async Task<GetApartmentDetailResponseDTO> GetApartmentDetails(int ApartmentId)
        {
            var result = new GetApartmentDetailResponseDTO();

            var apartment = await GetApartment(ApartmentId);
            result.ApartmentDescription = apartment.Description;
            result.ApartmentName = apartment.Name;
            result.ApartmentAddress = apartment.Address;
            result.ApartmentCity = apartment.City;
            result.TotalCount = apartment.NumberOfUsers;
            result.ApartmentPrice = apartment.Price;
            //Get Count in
            var ExistingStudents = _usersApartmentsServices.GetCountStudentsInApartment(ApartmentId);
            result.StudnetExistingIn = ExistingStudents;
            //Get Apartment Files
            var images = _imagesService.GetApartmentImgs(ApartmentId); // Cover Image?
            List<string> tempURLS= new List<string>();
            foreach (var image in images)
            {
                tempURLS.Add(image.ImageUrl);//null?
            }
            result.ApartmentsFiles= tempURLS;
            var Video = _videosService.GetApartmentdVideo(ApartmentId);
            result.ApartmentsFiles.Add(Video.FirstOrDefault());
            //Likes
            var likes = _reactServices.GetApartmentReacts(ApartmentId);
            result.ApartmentLikes = likes;
            //Owner!
            var user = await _authService.FindByIdAsync(apartment.OwnerId);
            result.OwnerName = user.UserName;
            result.OwnerImageUrl = user.imageUrl;
            //Owner Apartments Count
            result.OwnerApartmentCount = GetOwnerApartments(user.Id).Count();
            //Comments
            var comments = _commentServices.GetApartmentComment(apartment.Id);
            result.ApartmentComments =await HandleApartmentComments(comments);
            return result;

        }

        #endregion

        #endregion

        #region Private Functions
        private async Task<List<ApartmentComments>> HandleApartmentComments(List<UserApartmentsComment> comments)
        {
            var result = new List<ApartmentComments>();
            foreach (var comment in comments)
            {
                var temp = new ApartmentComments()
                {
                    CommentID = comment.Id,
                    CommentValue = comment.Comment,
                    UserCommentName = comment.user.UserName,
                    UserCommentImageUrl = comment.user.imageUrl
                };
                result.Add(temp);
            }
            return result;
        }
        #endregion



    }
}
