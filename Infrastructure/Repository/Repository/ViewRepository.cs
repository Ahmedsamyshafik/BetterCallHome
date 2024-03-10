using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class ViewRepository : GenericRepositoryAsync<View>, IViewRepository
    {
        private readonly DbSet<View> view;
        public ViewRepository(AppDbContext db) : base(db)
        {
            view = db.Set<View>();
        }
    }
}
