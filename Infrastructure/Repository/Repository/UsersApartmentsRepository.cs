using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class UsersApartmentsRepository : GenericRepositoryAsync<UsersApartments>, IUsersApartmentsRepository
    {
        private readonly DbSet<UsersApartments> usersApartments;
        public UsersApartmentsRepository(AppDbContext db) : base(db)
        {
            usersApartments = db.Set<UsersApartments>();
        }
    }
}
