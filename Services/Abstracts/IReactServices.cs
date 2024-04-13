using Domin.Models;

namespace Services.Abstracts
{
    public interface IReactServices
    {
        Task<string> AddReactAsync(UserApartmentsReact react);
        Task<bool> CanReactOrNo(string userId, int apartmentId);
        Task<List<UserApartmentsReact>> GetApartmentsReacts(List<int> apartmentids);
        int GetApartmentReacts(int apartmentId);
    }
}
