using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(255)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(255)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
