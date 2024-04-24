using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class UsersApartments
    {
        [Key]
        public int id { set; get; }
        public string UserID { set; get; }
        [ForeignKey("ApartmentFK")]
        public int ApartmentID { set; get;}

        public DateTime DateTime { set; get; }= DateTime.UtcNow;

        [ForeignKey("UserID")]
        public ICollection<ApplicationUser>? Users { set; get; }

       
        public virtual Apartment? ApartmentFK { set; get; }

    }
}
