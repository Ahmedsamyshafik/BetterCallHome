using AutoMapper;
using Domin.Constant;
using Domin.Models;

using Infrastructure.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Abstracts;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        #region InJect
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IGlobalErrorResponse _GlobalError;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUploadingMedia _media;


        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMailService mailService,
            IMapper mapper, IGlobalErrorResponse globalError, IWebHostEnvironment hostingEnvironment, IUploadingMedia media)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _mapper = mapper;
            _GlobalError = globalError;
            _hostingEnvironment = hostingEnvironment;
            _media = media;
        }
        #endregion

        #region Login/Register

        public async Task<UserDTO> RegisterAsync(RegisterDTO model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null || await _userManager.FindByNameAsync(model.UserName) != null)
            { // User with same email
                return new UserDTO { Message = "Email Or Name is Already Registered" };
            }
            //maping from RegisterDTO to ApplicationUser
            var user = _mapper.Map<ApplicationUser>(model);
            //Create User  
            var result = await _userManager.CreateAsync(user, model.Password);
            // Complete Comment and mapping
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
            string url = $"{_configuration["Website:AppUrl"]}" + $"api/Account/confirmemailForBackEnd?userid={user.Id}&token={validEmailToken}";
            SendConfirmEmail(user.Email, url);//Error?
            await _userManager.AddToRoleAsync(user, Constants.UserRole);
            //Create JWT
            var JwtSecuirtyToken = await CreateJwtToken(user);
            var res = _mapper.Map<UserDTO>(user);
            res.Expier = JwtSecuirtyToken.ValidTo;
            res.Role = Constants.UserRole;
            res.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuirtyToken);
            res.IsAuthenticated = true;
            return res;
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
                expires: DateTime.Now.AddDays(7), // Expire date..
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

            var returned = _mapper.Map<UserDTO>(user);
            //roleing
            if (roles.Contains(Constants.AdminRole))
            {
                returned.Role = Constants.AdminRole;
            }
            else if (roles.Contains(Constants.OwnerRole))
            {
                returned.Role = Constants.OwnerRole;
            }
            else
            {
                returned.Role = Constants.UserRole;
            }

            //returned.Roles = roles.ToList();
            returned.IsAuthenticated = true;
            returned.Expier = JwtSecuirtyToken.ValidTo;
            returned.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuirtyToken);

            return returned;
        }


        #endregion

        #region Services

        public bool NameIsExist(string name, string email)
        {
            var listofUsers = _userManager.Users.AsQueryable().Where(x => x.UserName == name && x.Email != email);
            if (listofUsers.Any()) return true;
            return false;
        }

        public async Task<string> UpdateStudentandOwnerProfile(ApplicationUser user, IFormFile? img)
        {
            var realUser = await _userManager.FindByEmailAsync(user.Email);
            if (realUser == null) return "No Account with This Email!";
            //check image
            if (img != null)
            {
                var returned = await _media.UploadFileAsync(img, Constants.EditProfilePicture);
                realUser.imagePath = returned.Path;
                realUser.imageName = returned.Name;
            }
            //Name? Falidation about name here?
            var ExistOrNot = NameIsExist(user.UserName, user.Email);
            if (ExistOrNot) return "Exist Name Already!";
            //adding all info
            realUser.UserName = user.UserName;
            realUser.PhoneNumber = user.PhoneNumber;
            //update
            var result = await _userManager.UpdateAsync(realUser);
            //return
            if (result.Succeeded) return "Success";
            return _GlobalError.ErrorCode(result);
        }

        public async Task<ReturnedUserDataDto> GetUserData(string ViewerID, string ViewedID)
        {
            var userdb = await _userManager.FindByIdAsync(ViewerID);
            if (userdb == null) return new ReturnedUserDataDto { Message = "User Not Founded" };
            userdb.Counter++;
            var result = await _userManager.UpdateAsync(userdb);
            if (!result.Succeeded) return new ReturnedUserDataDto { Message = "Faild" };
            if (ViewedID == ViewerID) // Not increase views by him self
                return new ReturnedUserDataDto { Message = "Success", user = userdb };
            return new ReturnedUserDataDto { Message = "Success", user = userdb, OperationCount = userdb.Counter };

        }


        #endregion

        #region Email

        public async Task<UserManagerResponseDTO> ConfirmEmail(string userId, string token)
        {
            var result = await _mailService.ConfirmEmail(userId, token);
            return result;

            #region Applying Single Responsibility Principle..  
            ////  var user = await _userManager.FindByIdAsync(userId);
            ////  if (user == null)
            ////      return new UserManagerResponseDTO
            ////      {
            ////          IsSuccess = false,
            ////          Message = "User not found"
            ////      };
            ////  //Decoding Token
            ////  var decodedToken = WebEncoders.Base64UrlDecode(token);
            ////  string normalToken = Encoding.UTF8.GetString(decodedToken);
            ////  // Success Confirm
            ////  var result = await _userManager.ConfirmEmailAsync(user, normalToken);
            //  if (result.Succeeded)
            //      return new UserManagerResponseDTO
            //      {
            //          Message = "Email confirmed successfully!",
            //          IsSuccess = true,
            //      };
            //  // Invaild Confirm
            //  return new UserManagerResponseDTO
            //  {
            //      IsSuccess = false,
            //      Message = "Email did not confirm",
            //      Errors = result.Errors.Select(e => e.Description)
            //  };
            #endregion 


        }

        private void SendConfirmEmail(string EmailSentTo, string url)
        {

            _mailService.SendConfirmEmail(EmailSentTo, url);

            #region Applying Single Responsibility Principle..  

            //var email = new MimeMessage();
            //email.From.Add(new MailboxAddress(_configuration["WebSite:FromName"], _configuration["WebSite:FromEmail"]));
            //email.To.Add(MailboxAddress.Parse(EmailSentTo));
            //email.Subject = "Account Confirm";
            //var htmlPage = $"<h1>Confirm your email</h1><p>Please confirm your email by <a href='{url}'>Clicking here</a></p>";
            //email.Body = new TextPart(TextFormat.Html) { Text = htmlPage };
            //using var smtp = new MailKit.Net.Smtp.SmtpClient();
            //smtp.Connect("smtp.gmail.com", 465, true);

            //smtp.Authenticate(userName: _configuration["WebSite:FromEmail"], password: _configuration["WebSite:Password"]);
            //smtp.Send(email);
            //smtp.Disconnect(true);
            #endregion

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
            Random generator = new Random();
            string Code = generator.Next(0, 1000000).ToString("D6");
            //Save in DB
            user.CodeConfirm = Code;
            var updating = await _userManager.UpdateAsync(user);
            if (!updating.Succeeded)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "Faild"
                };
            }
            // Send Email ! 
            _mailService.SendResetPassword(email, Code);
            return new UserManagerResponseDTO
            {
                IsSuccess = true,
                Message = "Reset password url has been sent"
            };
        }

        public async Task<UserManagerResponseDTO> ConfirmForgetPassword(string email, string code)
        {
            //user?
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "this email is invalid"
                };
            }
            var OriginalCode = user.CodeConfirm;
            if (OriginalCode.Equals(code))
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "Success"
                };
            }
            else
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "Not Same Code!"
                };

            }
        }

        public async Task<UserManagerResponseDTO> ResetPassword(string email, string Password, string ConfrimPassword)
        {
            //user
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "this email is invalid"
                };
            }
            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "Some thing Gone Wrong!"
                };
            }
            var response = await _userManager.AddPasswordAsync(user, Password);
            if (response.Succeeded)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            else
            {

                string msg = "";
                foreach (var x in response.Errors)
                {
                    msg += $" '{x.Description}',";
                }
                return new UserManagerResponseDTO
                {
                    IsSuccess = true,
                    Message = msg
                };
            }
            //pass
            throw new NotImplementedException();
        }

        public async Task<UserManagerResponseDTO> ChangePassword(string email, string oldPassword, string newPassword)
        {
            //user
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "this email is invalid"
                };
            }
            //change password
            //check?
            var pass = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!pass)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "invalid password"
                };
            }
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                string msg = "";
                foreach (var x in result.Errors)
                {
                    msg += $" '{x.Description}', ";
                }
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = msg
                };
            }
            else
            {
                var res = await _userManager.UpdateAsync(user);
                if (!res.Succeeded)
                {
                    string msgs = "";
                    foreach (var x in result.Errors)
                    {
                        msgs += $" '{x.Description}', ";
                    }
                    return new UserManagerResponseDTO
                    {
                        IsSuccess = false,
                        Message = msgs
                    };
                }
                return new UserManagerResponseDTO
                {
                    IsSuccess = true,
                    Message = "Success"
                };
            }
        }



        //_mailService.SendResetPassword(EmailSentTo, url);

        #region comments
        //private void SendResetPassword(string EmailSentTo, string url)
        //{
        //    _mailService.SendResetPassword(EmailSentTo, url);

        //    #region Applying Single Responsibility Principle..  
        //    //var email = new MimeMessage();
        //    //email.From.Add(new MailboxAddress(_configuration["WebSite:FromName"], _configuration["WebSite:FromEmail"]));
        //    //email.To.Add(MailboxAddress.Parse(EmailSentTo));
        //    //email.Subject = "Account Confirm";
        //    //var htmlPage = $"<h1>Reset Password</h1><p>Reset your password by <a href='{url}'>Clicking here</a></p>";
        //    //email.Body = new TextPart(TextFormat.Html) { Text = htmlPage };
        //    //using var smtp = new MailKit.Net.Smtp.SmtpClient();
        //    //smtp.Connect("smtp.gmail.com", 465, true);

        //    //smtp.Authenticate(userName: _configuration["WebSite:FromEmail"], _configuration["WebSite:Password"]);
        //    //smtp.Send(email);
        //    //smtp.Disconnect(true);

        //}

        #endregion




        #endregion









    }
}
