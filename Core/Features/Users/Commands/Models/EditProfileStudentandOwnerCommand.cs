﻿using Core.Bases;
using Infrastructure.CustomValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class EditProfileStudentandOwnerCommand : IRequest<Response<string>>
    {

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        //Custom Valid to Not repeat   by fluent validation?
        public string Name { get; set; }

        [Required]
        [PhoneValidation]
        public string Phone { get; set; }

        // public string? Address { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        public IFormFile? Picture { get; set; }

    }
}

//[DataType(DataType.Password)]
//[MinLength(6)]
//[MaxLength(255)]
//public string? OldPassword { get; set; }

//[DataType(DataType.Password)]
//[MinLength(6)]
//[MaxLength(255)]
//public string? NewPassword { get; set; }

//[DataType(DataType.Password)]
//[Compare("NewPassword")]
//[MinLength(6)]
//[MaxLength(255)]
//public string? ConfirmPassword { get; set; }
