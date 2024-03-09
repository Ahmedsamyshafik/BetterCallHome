using System.ComponentModel.DataAnnotations;

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
