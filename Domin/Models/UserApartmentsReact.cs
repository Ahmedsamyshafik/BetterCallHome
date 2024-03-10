namespace Domin.Models
{
    public class UserApartmentsReact
    {
        public int Id { get; set; }
        //  [ForeignKey("user")]
        public string UserId { get; set; }
        // [ForeignKey("Apartment")]
        public int ApartmentId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public ApplicationUser? user { get; set; }
        public Apartment? Apartment { get; set; }
    }
}
