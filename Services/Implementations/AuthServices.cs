using AutoMapper;
using Domin.Constant;
using Domin.Models;

using Infrastructure.DTO;
using Infrastructure.DTO.Owner;
using Infrastructure.DTO.Student;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IGlobalErrorResponse _GlobalError;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUploadingMedia _media;
        private readonly IImagesServices _image;
        private readonly IApartmentServices _apartment;
        private readonly IUserApartmentsRequestsService _requestsService;
        private readonly IUsersApartmentsServices _usersApartments;


        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMailService mailService,
            IMapper mapper, IGlobalErrorResponse globalError, IWebHostEnvironment hostingEnvironment, IUploadingMedia media
            , RoleManager<IdentityRole> roleManager, IImagesServices image, IApartmentServices apartment, IUserApartmentsRequestsService requestsService
            , IUsersApartmentsServices usersApartments)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _mapper = mapper;
            _GlobalError = globalError;
            _hostingEnvironment = hostingEnvironment;
            _media = media;
            _roleManager = roleManager;
            _image = image;
            _apartment = apartment;
            _requestsService = requestsService;
            _usersApartments = usersApartments;
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
            user.CodeConfirm = string.Empty;
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
            await _userManager.UpdateAsync(user);//To Make Sure thier is id!
            //ConfirmEmail
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
            string url = $"{_configuration["Website:AppUrl"]}" + $"api/Authentication/confirmemailForBackEnd?userid={user.Id}&token={validEmailToken}";
            _mailService.SendConfirmEmail(user.Email, url);//Error?
            //Seed Role
            var y = await _userManager.AddToRoleAsync(user, model.UserType);
            await _userManager.UpdateAsync(user);
            //Create JWT
            //var JwtSecuirtyToken = await CreateJwtToken(user);
            var res = _mapper.Map<UserDTO>(user);
            //  res.Expier = JwtSecuirtyToken.ValidTo;
            res.Role = model.UserType;//response ?
            //res.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecuirtyToken);
            res.IsAuthenticated = true;
            res.UserId = user.Id;
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
            //check Confirming
            if (!user.EmailConfirmed)
            {
                returnedUser.Message = "Not Confirmed!";
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
            returned.UserId = user.Id;
            return returned;
        }


        #endregion

        #region Services(NameIsExist,UserIsExist,Up,UpdateProfile,GetUserData,GetAllUsers)

        public bool NameIsExist(string name, string email)
        {
            var listofUsers = _userManager.Users.AsQueryable().Where(x => x.UserName == name && x.Email != email);
            if (listofUsers.Any()) return true;
            return false;
        }
        public async Task<bool> UserISExist(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            return true;

        }
        public async Task<bool> UserAndApartmentISExist(string userId, int apartmentID)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var apartment = await _apartment.GetApartment(apartmentID);
            if (user == null || apartment == null) return false;
            return true;

        }

        public async Task<string> UpdateProfile(ApplicationUser user, IFormFile? img, string requestSchema, HostString hostString)
        {
            var realUser = await _userManager.FindByEmailAsync(user.Email);
            if (realUser == null) return "No Account with This Email!";
            if (!realUser.EmailConfirmed) return "Not Confirmed!";
            //check image
            if (img != null)
            {
                //upload => return path , name ,Folder File
                var returned = await _media.SavingImage(img, requestSchema, hostString, Constants.EditProfilePicture);
                if (!returned.success) return $"Error=> {returned.message}";
                realUser.imageUrl = returned.message;
                realUser.ImageName = returned.name;
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
            var emailConfirm = await _userManager.FindByIdAsync(ViewerID);
            if (!emailConfirm.EmailConfirmed) return new ReturnedUserDataDto() { Message = "Not Confirmed!" };
            var userdb = await _userManager.FindByIdAsync(ViewedID);
            if (userdb == null) return new ReturnedUserDataDto { Message = "User Not Founded" };
            if (ViewedID != ViewerID)
            {
                userdb.Counter++;
                var result = await _userManager.UpdateAsync(userdb);
                if (!result.Succeeded) return new ReturnedUserDataDto { Message = "Faild" };
            }
            return new ReturnedUserDataDto { Message = "Success", user = userdb, OperationCount = userdb.Counter };


        }

        public async Task<string> GetUserMaxRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Constants.AdminRole))
            {
                return Constants.AdminRole;
            }
            if (roles.Contains(Constants.OwnerRole))
            {
                return Constants.OwnerRole;
            }
            if (roles.Contains(Constants.UserRole))
            {
                return Constants.UserRole;
            }

            return "";
        }

        public async Task<List<GetAllUsersResponse>> GetAllUsers(string? search)
        {
            var usersList = await _userManager.Users.ToListAsync();
            if (search != null)
            {
                usersList = usersList.Where(x => x.UserName.Contains(search)).ToList();
            }
            var response = new List<GetAllUsersResponse>();

            foreach (var user in usersList)
            {

                var userRole = await _userManager.GetRolesAsync(user);
                if (userRole.Contains(Constants.AdminRole)) continue;
                var res = new GetAllUsersResponse();
                if (userRole.Contains(Constants.OwnerRole))
                {
                    res.Role = Constants.OwnerRole;
                }
                else
                {
                    res.Role = Constants.UserRole;
                }
                res.Phone = user.PhoneNumber;
                res.Name = user.UserName;
                res.Image = user.imageUrl;
                res.userId = user.Id;
                response.Add(res);
            }
            return response;
        }

        #endregion

        #region (DeleteUserFromAdmin , GetStudnentsRequestsToApartment , GetOwner'sStudnets , GetUserByID)

        public async Task<string> DeleteUser(string userId)
        {
            //check id?
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "not found";
            if (!user.EmailConfirmed) return "Not Confirmed!";

            _image.DeleteAccountPic(user.ImageName);

            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole.Contains(Constants.UserRole))
            {//if student?=> Delete from apartment ! 

                //Delete From Apartment?!!*** TODO when it
            }
            if (userRole.Contains(Constants.OwnerRole))
            { //if Owner?=> Delete img apartments + students from apartment
                //Get owner apartments

                
                var apartments = await _apartment.GetOwnerApartmentsAsList(userId);

                if (apartments.Count() > 0)
                {
                    foreach(var apSt in apartments)
                    {
                        //check Studnets?
                        var Std = _usersApartments.AnyStudnets(apSt.Id);
                        if (Std) return "Stduent";
                    }
                    foreach (var apartment in apartments)
                    {
                        //check Studnets?

                        var x = await _apartment.GetApartment(apartment.Id);
                        if (x != null)
                        {
                            await _apartment.DeleteApartmentFilesOnly(apartment);
                            //  await _apartment.DeleteApartmentAsync(apartment);
                        }


                    }
                }

                //Delete Students!!!*** ToDo when it
            }
            //Delete
            await _userManager.DeleteAsync(user);
            return "";
        }

        public async Task<List<ApartmentRequestsResponse>> GetStudentRequestApartments(string OwnerId)
        {
            //apartments to owner
            var apartments = await _apartment.GetOwnerApartmentsAsList(OwnerId);
            //get records from request table match owner ids//
            var records = _requestsService.GetOwnerRequests(OwnerId);

            //mapping (Get Student Data!) // check?
            var ListOfRequests = _mapper.Map<List<ApartmentRequestsResponse>>(records);

            return ListOfRequests;
        }
        #region MyRegion
        //public async Task<List<GetOwnerStudentsResponse>> GetOwnerStudents(string ownerId)
        //{
        //    List<GetOwnerStudentsResponse> result = new();
        //    //loop in apartments which have ownerid
        //    var apartments = await _apartment.GetOwnerApartmentsAsList(ownerId);
        //    //loop in students which have apartmentid
        //    List<ApplicationUser> users = new();
        //    foreach (var apartment in apartments)
        //    { //null => New Table
        // //       users.Add(_userManager.Users.Include(x => x.CurrentLivingIn).Where(x => x.CurrentLivingInId == apartment.Id).FirstOrDefault());
        //    }
        //    //mapping
        //    foreach (var user in users)
        //    {
        //        var temp = new GetOwnerStudentsResponse();
        //        temp.phone = user.PhoneNumber;
        //        temp.email = user.Email;
        //        temp.imageUrl = user.imageUrl;
        //        temp.name = user.UserName;
        //      //  temp.ApartmentID = (int)user.CurrentLivingInId;
        //       // temp.ApartmentName = // New Table

        //        result.Add(temp);
        //    }
        //    return result;
        //}
        #endregion



        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public IQueryable returnApartmentRequestsResponseAsQueryable(List<ApartmentRequestsResponse> list)
        {
            return list.AsQueryable();
        }
        #endregion

        #region Housing

        public async Task<string> AssignStudentToApartment(string UserId, int ApartmentId) //New Table And Saving
        {
            var user = await _userManager.FindByIdAsync(UserId); // USer Apartment
            //   user.CurrentLivingInId = ApartmentId;
            await _userManager.UpdateAsync(user);
            return "Success";
        }

        public async Task<List<GetOwnerStudentsResponse>> GetOwnerStudentsResponses(List<UsersApartments> lop)
        {
            List<GetOwnerStudentsResponse> responses = new();
            foreach (var item in lop)
            {
                if (item == null) continue;
                var apartment=await _apartment.GetApartment(item.ApartmentsID);
                var user=await _userManager.FindByIdAsync(item.UserID);
                responses.Add(new GetOwnerStudentsResponse { ApartmentID=item.ApartmentsID,ApartmentName=apartment.Name ,email=user.Email,imageUrl=user.imageUrl,name=user.UserName,phone=user.PhoneNumber});
            }
            return responses;
        }

        #endregion

        #region Email

        public async Task<UserManagerResponseDTO> ConfirmEmail(string userId, string token)
        {
            var result = await _mailService.ConfirmEmail(userId, token);
            return result;
        }


        #endregion

        #region Password

        public async Task<UserManagerResponseDTO> ForgetPassword(string email)
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
                    IsSuccess = true,
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
            if (!user.EmailConfirmed)
            {
                return new UserManagerResponseDTO
                {
                    IsSuccess = false,
                    Message = "Not Confirmed!"
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


        #endregion
    }
}
