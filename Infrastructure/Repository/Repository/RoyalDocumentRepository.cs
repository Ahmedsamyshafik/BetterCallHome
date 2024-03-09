using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class RoyalDocumentRepository : GenericRepositoryAsync<RoyalDocument>, IRoyalDocumentRepository
    {
        private readonly DbSet<RoyalDocumentRepository> royal;
        public RoyalDocumentRepository(AppDbContext db) : base(db)
        {
            royal = db.Set<RoyalDocumentRepository>();
        }
    }
}
