using Domin.Models;
using Infrastructure.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;

namespace Services.Implementations
{
    public class UsersApartmentsServices : IUsersApartmentsServices
    {
        #region Fields
        private readonly IUsersApartmentsRepository _Repo;
        private readonly UserManager<ApplicationUser> _UserManager;
        #endregion

        #region Ctor
        public UsersApartmentsServices(IUsersApartmentsRepository Repo, UserManager<ApplicationUser> userManager
            )
        {
            _Repo = Repo;
            _UserManager = userManager;

        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(string userid, int apartmentid)
        {
            var x = new UsersApartments() { ApartmentID = apartmentid, UserID = userid, DateTime = DateTime.UtcNow };
            await _Repo.AddAsync(x);
            return "";
        }

        public async Task<List<UsersApartments>> GetRecordsByApartmentdIds(List<int> ids)
        {
            List<UsersApartments> usersid = new List<UsersApartments>();
            foreach (var id in ids)
            {
                var userid = _Repo.GetTableNoTracking().Where(x => x.ApartmentID == id).FirstOrDefault();
                usersid.Add(userid);
            }
            return usersid;
        }

        public int GetCountStudentsInApartment(int ApartmentID)
        {
            return _Repo.GetTableNoTracking().Where(x => x.ApartmentID.Equals(ApartmentID)).Count();
        }

        public bool AnyStudnets(int apartmentId)
        {
            return _Repo.GetTableNoTracking().Where(x => x.ApartmentID == apartmentId).Any();
        }

        public async Task<ReturnedNotifyForStudent> GetNotyForAcceptStudentAsync(string userID)
        {
            var rec = _Repo.GetTableNoTracking().Where(x => x.UserID == userID).FirstOrDefault();

            var returend = new ReturnedNotifyForStudent();
            if (rec == null)
            {
                returend.isEixst = false;

            }
            else
            {
              //  var userid = _ApartmentServices.GetApartment(rec.ApartmentsID).Result.OwnerId;
                var user = await _UserManager.FindByIdAsync(rec.UserID);
                returend = new ReturnedNotifyForStudent()
                {
                    isEixst = true,
                    date = rec.DateTime,
                    OperationId = rec.id,
                    Type = "Accept",
                    apartmentId=rec.ApartmentID
                  
                };
            }



            return returend;
        }
        #endregion

        public class ReturnedNotifyForStudent
        {
            public bool isEixst { get; set; }
            public string Type { get; set; }
            public int apartmentId { get; set; }
            public string? OwnerID { get; set; }
            public DateTime? date { get; set; }
            public int OperationId { get; set; }
        }

    }
}
