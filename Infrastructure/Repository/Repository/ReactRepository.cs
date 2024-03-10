using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class ReactRepository : GenericRepositoryAsync<UserApartmentsReact>, IReactRepository
    {
        private readonly DbSet<UserApartmentsReact> likes;
        public ReactRepository(AppDbContext db) : base(db)
        {
            likes = db.Set<UserApartmentsReact>();
        }
    }
}
