using Infrastructure.CustomValidation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Users.Commands.Models
{
    public class EditProfileDTO
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
