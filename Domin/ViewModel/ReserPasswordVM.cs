using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.ViewModel
{
    public class ReserPasswordVM
    {
        [Required]
        public string Token { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100,MinimumLength =5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
