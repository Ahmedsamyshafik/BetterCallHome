using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ApartmentServices : IApartmentServices
    {
        #region Fields
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IImagesServices _imagesService;
        private readonly IRoyalServices _royalServices;
        private readonly IVideosServices _videosService;
        #endregion

        #region Ctor
        public ApartmentServices(IApartmentRepository apartmentRepository, IImagesServices imagesService,
            IRoyalServices royalServices, IVideosServices videosService)
        {
            _apartmentRepository = apartmentRepository;
            _imagesService = imagesService;
            _royalServices = royalServices;
            _videosService = videosService;
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

        public IQueryable<ApartmentPaginationPending> getpaginate(string? search)
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

                temp.OwnerName = apart.Owner.UserName;
                temp.OwnerImage = apart.Owner.imageUrl;



                var images = imgs.Where(x => x.ApartmentID == apart.Id).ToList();//list of apartments
                List<string> urls = new List<string>();
                urls.Add(apart.CoverImageUrl);

                foreach (var image in images)
                {
                    string url = image.ImageUrl;
                    if (url != null) urls.Add(url);

                }
                foreach (var video in videos)
                {
                    string url = video.VideoUrl;
                    if (url != null) urls.Add(url);
                }
                foreach (var royal in royals)
                {
                    string url = royal.ImageUrl;
                    if (url != null) urls.Add(url);
                }

                temp.ApartmentsPics = urls;

                // temp.ApartmentsPics.AddRange(videos); // imgs 



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
        #endregion



    }
}
