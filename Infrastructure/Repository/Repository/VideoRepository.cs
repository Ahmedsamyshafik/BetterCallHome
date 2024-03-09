using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class VideoRepository : GenericRepositoryAsync<ApartmentVideo>, IVideoRepository
    {
        private readonly DbSet<ApartmentVideo> videos;
        public VideoRepository(AppDbContext db) : base(db)
        {
            videos = db.Set<ApartmentVideo>();
        }
    }
}
