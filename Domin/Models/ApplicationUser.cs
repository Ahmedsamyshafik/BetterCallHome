using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class ApplicationUser : IdentityUser
    {
        // name , phone , Email
        public string College { get; set; }
        public string University { get; set; }                          
        public int Age { get; set; }
        public string  Gender{ get; set; }
        //[ForeignKey("UserApartment")]
        public int UserApartmentId { get; set; }//

        public virtual ICollection<UserApartment> UserApartment { get; set; }


    }
}
