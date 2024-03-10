using Domin.Models;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repository.IRepository
{
    public interface INotifyRepository : IGenericRepositoryAsync<NotificationTransaction>
    {
    }
}
