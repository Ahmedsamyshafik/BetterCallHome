using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domin.Models
{
    public class ApplicationUser : IdentityUser
    {
        // name , phone , Email
        public string? College { get; set; }
        public string? University { get; set; }
        public string? Address { get; set; }
        public string Gender { get; set; }
        public string? imagePath { get; set; }
        public string? imageName { get; set; }
        public int Counter { get; set; }


        // [ForeignKey("Apartments")]//Owner
        // public int UserApartmentId { get; set; }//
        [InverseProperty(nameof(Apartment.Owner))]
        public virtual ICollection<Apartment>? OwnedApartment { get; set; }//Owner


        // [InverseProperty(nameof(Models.Apartment.StudentsApartment))]
        [ForeignKey("Apartment")]// Student
        public int? CurrentLivingInId { get; set; }
        public virtual Apartment? CurrentLivingIn { get; set; }

        public virtual ICollection<View>? Viewers { get; set; }
        public virtual ICollection<UserApartmentsComment>? Comments { get; set; }
        public virtual ICollection<UserApartmentsReact>? Reacts { get; set; }

    }
}
