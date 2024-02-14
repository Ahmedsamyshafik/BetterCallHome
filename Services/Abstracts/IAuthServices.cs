using Domin.ViewModel;
using Infrastructure.DTO;

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
        Task<UserDTO> LoginForAdmin(LoginDTO model);
        Task<UserDTO> LoginForOwners(LoginDTO model);
    }
}
