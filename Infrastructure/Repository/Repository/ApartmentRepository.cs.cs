using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class ApartmentRepository : GenericRepositoryAsync<Apartment>, IApartmentRepository
    {
        private readonly DbSet<Apartment> apartments;
        public ApartmentRepository(AppDbContext db) : base(db)
        {
            apartments = db.Set<Apartment>();
        }
    }
}
