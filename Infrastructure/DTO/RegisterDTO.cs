using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class RegisterDTO
    {
        // Valid..
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(11)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(10)]
        public int Age { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Colleage { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string University { get; set; }

        [Required]
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
        public string COPassword { get; set; }

    }
}
