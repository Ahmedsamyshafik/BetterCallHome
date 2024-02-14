using Microsoft.AspNetCore.Identity;

namespace Domin.Models
{
    public class ApplicationUser : IdentityUser
    {
        // name , phone , Email
        public string? College { get; set; }
        public string? University { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        //[ForeignKey("UserApartment")]
        public int UserApartmentId { get; set; }//

        public virtual ICollection<UserApartment> UserApartment { get; set; }


    }
}
