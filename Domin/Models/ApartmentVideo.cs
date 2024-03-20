using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class ApartmentVideo
    {
        [Key]
        public int Id { get; set; }
        public string VideoUrl { get; set; }
        public string Name { get; set; }
        [ForeignKey("Apartment")]
        public int ApartmentID { get; set; }
        public Apartment Apartment { get; set; }
    }
}
