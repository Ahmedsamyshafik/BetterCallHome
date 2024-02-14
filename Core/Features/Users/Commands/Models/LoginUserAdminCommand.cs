﻿using Core.Bases;
using Core.Features.Users.Commands.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class LoginUserAdminCommand : IRequest<Response<UserResponse>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; }
    }

}

