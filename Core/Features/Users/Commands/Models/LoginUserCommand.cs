﻿using Core.Bases;
using Core.Features.Users.Commands.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class LoginUserCommand : IRequest<Response<UserResponse>>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]

        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; }
      
        public IFormFile? image { get; set; }
    }
}
