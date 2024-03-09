using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class imagesRepository : GenericRepositoryAsync<ApartmentImages>, IimagesRepository
    {
        private readonly DbSet<ApartmentImages> images;
        public imagesRepository(AppDbContext db) : base(db)
        {
            images = db.Set<ApartmentImages>();
        }
    }
}
