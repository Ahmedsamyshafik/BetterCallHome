using Domin.Models;
using Domin.ViewModel;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Http;

namespace Services.Abstracts
{
    public interface IAuthService
    {

        Task<UserDTO> RegisterAsync(RegisterDTO model);
        Task<UserDTO> Login(LoginDTO model);
        Task<UserManagerResponseDTO> ConfirmEmail(string userId, string token);
        Task<UserManagerResponseDTO> ForgetPassword(string email);
        Task<UserManagerResponseDTO> ResetPasswordForEmail(ReserPasswordVM model);
        Task<UserManagerResponseDTO> ResetPassword(ResetPasswordDTO model);
        bool NameIsExist(string name, string email);
        Task<string> UpdateStudentandOwnerProfile(ApplicationUser user, IFormFile? img);
        Task<ReturnedUserDataDto> GetUserData(string ViewerID, string ViewedID);
    }
}
