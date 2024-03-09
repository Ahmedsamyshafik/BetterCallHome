namespace Domin.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //[ForeignKey("ApartmentServices")]
        public int ApartmentServicesId { get; set; }

        public virtual ICollection<ApartmentServices> ApartmentServices { get; set; }
    }
}
