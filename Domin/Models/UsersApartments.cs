using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class UsersApartments
    {
        [Key]
        public int id { set; get; }
        public string UserID { set; get; }

        public int ApartmentsID { set; get;}

        public DateTime DateTime { set; get; }= DateTime.UtcNow;

        [ForeignKey("UserID")]
        public ICollection<ApplicationUser>? Users { set; get; }

        [ForeignKey("ApartmentID")]
        public virtual Apartment? Apartment { set; get; }

    }
}
