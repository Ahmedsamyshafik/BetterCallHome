using Domin.Models;
using Infrastructure.Repository.IRepository;
using Services.Abstracts;

namespace Services.Implementations
{
    public class NotifyServices : iNotifyServices
    {
        #region Fields
        private readonly INotifyRepository _notifyRepository;

        #endregion
        #region Ctor
        public NotifyServices(INotifyRepository notifyRepository)
        {
            _notifyRepository = notifyRepository;
        }
        #endregion
        #region Handle Functions
        public async Task<string> AddAsync(NotificationTransaction notification)
        {
            var res = await _notifyRepository.AddAsync(notification);
            if (res != null) return "Success";
            return "Faild";
        }
        #endregion

    }
}
