using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class UserApartmentsRequests
    {
        public int Id { get; set; }
        public string UserID { get; set; }

        public string OwnerID { get; set; }
        public int ApartmentID { get; set; }


        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(UserID))]
        public ApplicationUser? ApplicationUser { get; set; }

        [ForeignKey(nameof(ApartmentID))]
        public Apartment? Apartment { get; set; }
    }
}
