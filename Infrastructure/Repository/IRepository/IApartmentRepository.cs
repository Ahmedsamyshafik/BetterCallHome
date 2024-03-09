using Domin.Models;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repository.IRepository
{
    public interface IApartmentRepository :IGenericRepositoryAsync<Apartment>
    {
    }
}
