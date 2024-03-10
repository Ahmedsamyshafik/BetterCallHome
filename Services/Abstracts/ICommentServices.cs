using Domin.Models;

namespace Services.Abstracts
{
    public interface ICommentServices
    {
        Task<string> AddCommentAsync(UserApartmentsComment comment);
        Task<bool> CanCommentOrNo(string userid, int apartmentid, int Counter);
        Task<List<UserApartmentsComment>> GetApartmentComments(List<int> apartmentids);
    }
}
