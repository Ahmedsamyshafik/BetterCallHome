using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class NotifyRepository : GenericRepositoryAsync<NotificationTransaction>, INotifyRepository
    {
        private readonly DbSet<NotificationTransaction> _notifications;
        public NotifyRepository(AppDbContext db) : base(db)
        {
            _notifications= db.Set<NotificationTransaction>();
        }
    }
}
