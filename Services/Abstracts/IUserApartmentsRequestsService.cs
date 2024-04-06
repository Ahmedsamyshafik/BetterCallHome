using Domin.Models;

namespace Services.Abstracts
{
    public interface IUserApartmentsRequestsService
    {
        Task<string> Add(UserApartmentsRequests request);
        List<UserApartmentsRequests> GetOwnerRequests(string ownerId);
        UserApartmentsRequests GetRecord(int id);
        Task<string> DeleteRecord(int id);
    }
}
