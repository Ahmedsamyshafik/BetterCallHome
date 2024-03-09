using Core.Features.Users.Commands.Models;
using Core.Features.Users.Commands.Results;
using Domin.Models;
using Infrastructure.DTO;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile
    {
        public void RegisterUserComandToResgisterDTo_CommandMapping()
        {
            // from RegisterCommand To RegisterDTO
            CreateMap<RegisterStudentCommand, RegisterDTO>();

        }
        public void UserDTOToUserResponse_CommandMapping()
        {
            //From UserDTO to UserResponse
            CreateMap<UserDTO, UserResponse>();
        }
        public void UserDtoToUserResponse_CommandMapping()
        {
            //From UserDTO to UserResponse
            CreateMap<UserDTO, UserResponse>();
        }
        public void RegisteDtoToApplicationUser_CommandMapping()
        {
            //From RegisterDTO to ApplicationUser
            CreateMap<RegisterDTO, ApplicationUser>().
                ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
        }
        public void ApplicationUserToRegisteDto_CommandMapping()
        {
            //From RegisterDTO to ApplicationUser
            CreateMap<ApplicationUser, RegisterDTO>().
                ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
        }
        public void ApplicationUserToUserDto_CommandMapping()
        {
            //From ApplicationUser to UserDTO  
            CreateMap<ApplicationUser, UserDTO>().
                ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
            //  ForMember(dest => dest.College, opt => opt.MapFrom(src => src.College));
        }
        public void loginUserCommandToLoginDTO_CommandMapping()
        {
            // mapping from loginUserCommand To LoginDTO
            CreateMap<LoginUserCommand, LoginDTO>();

        }
        public void loginUserAdminCommandToLoginDTO_CommandMapping()
        {
            // mapping from loginUserAdminCommand To LoginDTO
            CreateMap<LoginUserAdminCommand, LoginDTO>();

        }











    }
}
