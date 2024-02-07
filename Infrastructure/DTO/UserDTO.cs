using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }



        public int Age { get; set; }
        public string Colleage { get; set; }
        public string University { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }

        public string Message { get; set; }

        public bool IsAuthenticated { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }

        public DateTime Expier { get; set; }




    }
}
