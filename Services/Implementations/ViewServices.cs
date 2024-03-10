using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class ViewServices : IViewServices
    {
        private readonly IViewRepository _view;

        public ViewServices(IViewRepository view)
        {
            _view = view;
        }


        public async Task<string> AddView(View model)
        {
            var x = await _view.AddAsync(model);
            return "Done";
        }
    }
}
