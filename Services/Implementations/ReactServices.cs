using Domin.Models;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ReactServices : IReactServices
    {
        #region Fields
        private readonly IReactRepository _react;

        #endregion
        #region Ctor
        public ReactServices(IReactRepository react)
        {
            _react = react;
        }
        #endregion   
        #region Handle Functions

        public async Task<string> AddReactAsync(UserApartmentsReact react)
        {
            var x = await _react.AddAsync(react);
            if (x != null) return "Success";
            return "Faild";
        }

        public async Task<bool> CanReactOrNo(string userId, int apartmentId)
        {
            var exist = _react.GetTableNoTracking().Where(x => x.UserId == userId && x.ApartmentId == apartmentId);
            if (exist.Count() > 0) return false;
            return true;
        }

        public async Task<List<UserApartmentsReact>> GetApartmentsReacts(List<int> apartmentids)
        {
            var likes = new List<UserApartmentsReact>();
            foreach (var id in apartmentids)
            {
                var like = await _react.GetTableNoTracking().Where(x => x.ApartmentId == id).ToListAsync();
                if (like.Count() > 0)
                {
                    foreach (var react in like) likes.Add(react);
                }
            }
            return likes;
        }
        public int GetApartmentReacts(int apartmentId)
        {

            return   _react.GetTableNoTracking().Where(x => x.ApartmentId == apartmentId).Count();
                
             
        }
        #endregion


    }
}
