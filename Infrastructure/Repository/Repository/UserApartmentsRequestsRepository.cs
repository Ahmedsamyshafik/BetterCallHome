using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class UserApartmentsRequestsRepository :GenericRepositoryAsync<UserApartmentsRequests> ,IUserApartmentsRequestsRepository
    {
        private readonly DbSet<UserApartmentsRequests> requests;
        public UserApartmentsRequestsRepository(AppDbContext db) : base(db)
        {
            requests = db.Set<UserApartmentsRequests>();
        }

    }
}
