using Domin.Models;
using Infrastructure.Data;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repository
{
    public class CommentRepository : GenericRepositoryAsync<UserApartmentsComment>, ICommentRepository
    {
        private readonly DbSet<UserApartmentsComment> comment;
        public CommentRepository(AppDbContext db) : base(db)
        {
            comment = db.Set<UserApartmentsComment>();
        }




    }
}
