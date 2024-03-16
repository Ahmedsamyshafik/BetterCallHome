using Infrastructure.DTO;

namespace Services.Abstracts
{
    public interface IMailService
    {
        Task<UserManagerResponseDTO> ConfirmEmail(string userId, string token);
        void SendConfirmEmail(string EmailSentTo, string url);
        void SendResetPassword(string EmailSentTo, string code);
    }
}
