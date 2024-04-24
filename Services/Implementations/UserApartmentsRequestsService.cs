using AutoMapper;
using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;

namespace Services.Implementations
{
    public class UserApartmentsRequestsService : IUserApartmentsRequestsService
    {
        #region Fields
        private readonly IUserApartmentsRequestsRepository _requestsRepo;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public UserApartmentsRequestsService(IUserApartmentsRequestsRepository requestsRepo, IMapper mapper)
        {
            _requestsRepo = requestsRepo;
            _mapper = mapper;
        }
        #endregion

        #region Handle Functions

        public async Task<string> Add(UserApartmentsRequests request)
        {
            // mapping from student_Request To UserApartment
            await _requestsRepo.AddAsync(request);
            return "";
        }

        public List<UserApartmentsRequests> GetOwnerRequests(string ownerId)
        {
            return _requestsRepo.GetTableNoTracking().
                                                    Include(u => u.Apartment).
                                                    Include(u => u.ApplicationUser).
                                                    Where(x => x.OwnerID == ownerId).
                                                    ToList();
        }

        public UserApartmentsRequests GetRecord(int id)
        {
            return _requestsRepo.GetTableNoTracking().Where(x => x.Id == id).FirstOrDefault();
        }
        public async Task<string> DeleteRecord(int id)
        {
            await _requestsRepo.DeleteAsync(GetRecord(id));
            return "";
        }

        public bool CheckStudentRequestOtherApartment(string userID, int apartmentId)
        {
            return _requestsRepo.GetTableNoTracking().Where(x => x.UserID == userID && x.ApartmentID != apartmentId).Any();
        }
        #endregion
    }
}
