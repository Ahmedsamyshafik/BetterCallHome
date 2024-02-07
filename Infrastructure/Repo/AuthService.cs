﻿using Domin.Models;
using Infrastructure.DTO;
using Infrastructure.IRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MimeKit;
using MimeKit.Text;
using Domin.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using Domin.Constant;

namespace Infrastructure.Repo
{
    public class AuthService : IAuthService
    {
        #region InJect
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        #endregion

        #region Login/Register

        public async Task<UserDTO> RegisterAsync(RegisterDTO model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null || await _userManager.FindByNameAsync(model.UserName) != null)
            { // User with same email
                return new UserDTO { Message = "Email Or Name is Already Registered" };
            }
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                Age = model.Age,
                College = model.Colleage,
                University = model.University,
                Gender = model.Gender

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var Es = string.Empty;
                foreach (var error in result.Errors)
                {
                    Es += $"{error.Description} ,";
                }
                return new UserDTO { Message = Es };
            }
            //ConfirmEmail
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
            string url = $"{_configuration["AppUrl"]}" + $"api/Account/confirmemail?userid={user.Id}&token={validEmailToken}";
            SendConfirmEmail(user.Email, url);
            await _userManager.AddToRoleAsync(user, Constants.UserRole);
            var JwtSecuirtyToken = await CreateJwtToken(user);
            return new UserDTO
            {
                Email = user.Email,
                Expier = JwtSecuirtyToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { Constants.UserRole },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuirtyToken),
                UserName = user.UserName,
                Phone = model.Phone,
                Age = model.Age,
                Colleage = model.Colleage,
                University = model.University,
                Gender = model.Gender
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var SigingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(1), // Expire date..
                signingCredentials: SigingCredentials
                );
            var myToken = new
            {
                _token = new JwtSecurityTokenHandler().WriteToken(token), // To make Token Json
                expiration = token.ValidTo
            };
            return token;
        }

        public async Task<UserDTO> Login(LoginDTO model)
        {
            UserDTO returnedUser = new();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                returnedUser.Message = "Name Or Password is invalid..!";

                returnedUser.Email = model.Email;
                return returnedUser;
            }
            var JwtSecuirtyToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            returnedUser.Email = user.Email;
            returnedUser.UserName = user.UserName;
            returnedUser.Gender = user.Gender;
            returnedUser.Age = user.Age;
            returnedUser.Colleage = user.College;
            returnedUser.University = user.University;
            returnedUser.Phone = user.PhoneNumber;
            returnedUser.Roles = roles.ToList();
            returnedUser.IsAuthenticated = true;
            returnedUser.Expier = JwtSecuirtyToken.ValidTo;
            returnedUser.IsAuthenticated = true;
            returnedUser.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuirtyToken);

            return returnedUser;
        }

        public async Task<UserDTO> LoginForAdmin(LoginDTO model)
        {
            UserDTO returnedUser = new();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                returnedUser.Message = "Name Or Password is invalid..!";

                returnedUser.Email = model.Email;
                return returnedUser;
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Constants.AdminRole))
            {
              return await Login(model);
            }
            else
            {
                returnedUser.Message = "Name Or Password is invalid..!";

                returnedUser.Email = model.Email;
                return returnedUser;
            }
        }

        public async Task<UserDTO> LoginForOwners(LoginDTO model)
        {
            UserDTO returnedUser = new();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                returnedUser.Message = "Name Or Password is invalid..!";
                var r = await _userManager.CheckPasswordAsync(user, model.Password);
               
                returnedUser.Email = model.Email;
                return returnedUser;
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Constants.OwnerRole))
            {
                return await Login(model);
            }
            else
            {
                returnedUser.Message = "Name Or Password is invalid..!";
                returnedUser.IsAuthenticated = false;
                returnedUser.Email = model.Email;
                return returnedUser;
            }
        }
        #endregion

        #region Email

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

        private void SendConfirmEmail(string EmailSentTo, string url)
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

        #endregion

        #region Password

        public async Task<UserManagerResponseDTO> ForgetPassword(string email) // Controller
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "this email is invalid"
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            //-- Here Front URL
            string url = $"{_configuration["Website:AppUrl"]}" + $"ResetPasswordFromEmail?email={email}&token={validToken}";
            SendResetPassword(email, url); // Send Email ! 
            return new UserManagerResponseDTO
            {
                IsSuccess = true,
                Message = "Reset password url has been sent"
            };
        }
        // Controller  -- Resiting
        public async Task<UserManagerResponseDTO> ResetPasswordForEmail(ReserPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "email not found"
                };
            if (model.ConfirmPassword != model.NewPassword) return new UserManagerResponseDTO { IsSuccess = false, Message = "Confirm Password dose not match New Password..!" };
            //Decoding Token
            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
            if (result.Succeeded)
                return new UserManagerResponseDTO
                {
                    IsSuccess = true,
                    Message = "Password has been reset successfully!"
                };
            return new UserManagerResponseDTO()
            {
                IsSuccess = false,
                Message = "Something went wrnog :(",
                Errors = result.Errors.Select(e => e.Description).ToArray()
            };
        }

        private void SendResetPassword(string EmailSentTo, string url)
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


        public async Task<UserManagerResponseDTO> ResetPassword(ResetPasswordDTO model)
        {
            //Check Email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "No matched Email.."
                };
            }
            // Confirm!=New
            if (model.NewPassword != model.ConfirmPassword) return new UserManagerResponseDTO(){ IsSuccess = false, Message = "Confirm Password dose not match New Password..!" };
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword,model.NewPassword);
            if (result.Succeeded)
            {
                return new UserManagerResponseDTO() { IsSuccess=true , Message ="Password Updated Successfuily"};
            }
            else
            {
                string errorMessage = "";
                var errors = result.Errors.Select(e => e.Description).ToArray();
                if (errors.Length > 0)
                {
                    errorMessage = string.Join(", ", errors);  
                }
                return new UserManagerResponseDTO()
                {
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray(),
                    Message = $"thier are errors=> {errorMessage}",
                };
            }
        }
        #endregion
    }
}
