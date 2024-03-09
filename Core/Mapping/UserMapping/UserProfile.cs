﻿using AutoMapper;

namespace Core.Mapping.UserMapping
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {
            RegisterUserComandToResgisterDTo_CommandMapping();
            UserDTOToUserResponse_CommandMapping();
            UserDtoToUserResponse_CommandMapping();
            RegisteDtoToApplicationUser_CommandMapping();
            ApplicationUserToUserDto_CommandMapping();
            loginUserCommandToLoginDTO_CommandMapping();
            loginUserAdminCommandToLoginDTO_CommandMapping();
            ResetPasswordUserCommandToResetPasswordDto_CommandMapping();
            EditProfileCommandToApplicationUser_CommaneMapping();


        }
    }
}
