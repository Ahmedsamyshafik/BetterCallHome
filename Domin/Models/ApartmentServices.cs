using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Models
{
    public class ApartmentServices
    {
        [Key]
        public int Id { get; set; }
        public int ApartmentId { get; set; }//
        public int ServiceId { get; set; } // 

        public Apartment Apartment { get; set; }
        public Service Service { get; set; }                                                                        
    }
}
