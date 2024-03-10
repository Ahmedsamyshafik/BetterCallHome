using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ViewServices : IViewServices
    {
        #region Fields
        private readonly IViewRepository _view;

        #endregion

        #region Fields
        public ViewServices(IViewRepository view)
        {
            _view = view;
        }


        #endregion
        #region Fields

        public async Task<string> AddView(View model)
        {
            var x = await _view.AddAsync(model);
            return "Done";
        }

        public List<View> GetAccountViews(string userId)
        {
            var views = _view.GetTableNoTracking().Where(x => x.ViewedID == userId).ToList();
            return views;
        }
        #endregion


    }
}
