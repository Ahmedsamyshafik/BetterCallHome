using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class Apartment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Room { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string PDF { get; set; }
        public int NumberOfUsers { get; set; }
        public int Views { get; set; } // alot like that 

        public int ApartmentServicesId { get; set; }

        public int UserApartmentId { get; set; }//

        public virtual ICollection<ApartmentServices> ApartmentServices { get; set; }
        public virtual ICollection<UserApartment> UserApartment { get; set; }


    }
}
