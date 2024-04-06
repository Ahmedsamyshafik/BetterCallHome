using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class UsersApartmentsServices : IUsersApartmentsServices
    {
        #region Fields
        private readonly IUsersApartmentsRepository _Repo;
        #endregion

        #region Ctor
        public UsersApartmentsServices(IUsersApartmentsRepository Repo)
        {
            _Repo = Repo;
        }
        #endregion

        #region Handle Functions
        public async Task<string> AddAsync(string userid, int apartmentid)
        {
            var x = new UsersApartments() { ApartmentsID = apartmentid, UserID = userid, DateTime = DateTime.UtcNow };
            await _Repo.AddAsync(x);
            return "";
        }

        public async Task<List<UsersApartments>> GetRecordsByApartmentdIds(List<int> ids)
        {
            List<UsersApartments> usersid = new List<UsersApartments>();
            foreach (var id in ids)
            {
                var userid = _Repo.GetTableNoTracking().Where(x => x.ApartmentsID == id).FirstOrDefault();
                usersid.Add(userid);
            }
            return usersid;
        }

        public int GetCountStudentsInApartment(int ApartmentID)
        {
            return _Repo.GetTableNoTracking().Where(x=>x.ApartmentsID.Equals(ApartmentID)).Count();
        }

        public  bool AnyStudnets(int apartmentId)
        {
            return _Repo.GetTableNoTracking().Where(x=>x.ApartmentsID==apartmentId).Any();
        }
        #endregion
    }
}
