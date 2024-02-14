using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.IRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Text;

namespace Infrastructure.Repo
{
    public class MailService : IMailService
    {


        #region InJect
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public MailService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        #endregion



        public async Task<UserManagerResponseDTO> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            //Decoding Token
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            // Success Confirm
            var result = await _userManager.ConfirmEmailAsync(user, normalToken);
            if (result.Succeeded)
                return new UserManagerResponseDTO
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true,
                };
            // Invaild Confirm
            return new UserManagerResponseDTO
            {
                IsSuccess = false,
                Message = "Email did not confirm",
                Errors = result.Errors.Select(e => e.Description)
            };

        }

        public void SendConfirmEmail(string EmailSentTo, string url)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_configuration["WebSite:FromName"], _configuration["WebSite:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(EmailSentTo));
            email.Subject = "Account Confirm";
            var htmlPage = $"<h1>Confirm your email</h1><p>Please confirm your email by <a href='{url}'>Clicking here</a></p>";
            email.Body = new TextPart(TextFormat.Html) { Text = htmlPage };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, true);

            smtp.Authenticate(userName: _configuration["WebSite:FromEmail"], password: _configuration["WebSite:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendResetPassword(string EmailSentTo, string url)
        {


            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_configuration["WebSite:FromName"], _configuration["WebSite:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(EmailSentTo));
            email.Subject = "Account Confirm";
            var htmlPage = $"<h1>Reset Password</h1><p>Reset your password by <a href='{url}'>Clicking here</a></p>";
            email.Body = new TextPart(TextFormat.Html) { Text = htmlPage };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, true);

            smtp.Authenticate(userName: _configuration["WebSite:FromEmail"], _configuration["WebSite:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}
