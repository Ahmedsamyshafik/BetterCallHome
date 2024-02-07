using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class UserApartment 
    {
        public int Id { get; set; }

        public int ApplicationUserId { get; set; }

        //[ForeignKey("Apartment")]
        public int ApartmentId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Apartment Apartment { get; set; }


    }
}
