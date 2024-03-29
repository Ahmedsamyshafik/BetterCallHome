﻿using Core.Bases;
using Core.Features.Users.Commands.Results;
using Infrastructure.CustomValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class RegisterStudentCommand : IRequest<Response<UserResponse>>
    {

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [PhoneValidation]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //Address!!

        [Required]
        [GenderValidation]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [MinLength(6)]
        [MaxLength(255)]
        public string ConfirmPassword { get; set; }

        public IFormFile? imgae { get; set; }



        //[MinLength(2)]
        //[MaxLength(100)]
        //public string? College { get; set; }//


        //[MinLength(2)]
        //[MaxLength(100)]
        //public string? University { get; set; }//
        //[Required]
        //[AgeValidation]
        //public int Age { get; set; }//
    }
}
