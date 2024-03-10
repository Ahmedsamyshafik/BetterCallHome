using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class Apartment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public int Room { get; set; }
        public decimal Price { get; set; }
        public int NumberOfUsers { get; set; }
        public int Views { get; set; } // alot like that {Favourite}
        public bool Publish { get; set; }
        public string? CoverImageName { get; set; }

        // [ForeignKey("RoyalDocument")]
        public int Document { get; set; }//Royal Decument
        public virtual RoyalDocument? RoyalDocument { get; set; }



        public int ApartmentVideoID { get; set; }//Video
        ///[ForeignKey("ApartmentVideoID")]
        public virtual ApartmentVideo? ApartmentVideo { get; set; }

        // [InverseProperty(nameof(Models.ApplicationUser.Apartments))]
        [ForeignKey(nameof(Owner))]// ownerrr
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        

        //[ForeignKey("UsersApartment")]// Students
        [InverseProperty(nameof(Models.ApplicationUser.CurrentLivingIn))]
        //public int UserApartmentId { get; set; }
        public virtual ICollection<ApplicationUser>? StudentsApartment { get; set; }


        public virtual ICollection<UserApartmentsReact>? Reacts { get; set; }
        public virtual ICollection<UserApartmentsComment>? Comments { get; set; }

        #region Services?
        public int ApartmentServicesId { get; set; }
        public virtual ICollection<ApartmentServices>? ApartmentServices { get; set; }
        #endregion


    }
}
