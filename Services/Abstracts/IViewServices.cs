using Domin.Models;

namespace Services.Abstracts
{
    public interface IViewServices
    {
        public Task<string> AddView(View view);
        List<View> GetAccountViews(string userId);
    }
}
