namespace Core.Features.Apartments.Queries.Results
{
    public class GetApartmentForEditResponse
    {
        public string ApartmentId { get; set; }
        public string ApartmentName { get; set; }
        public string ApartmentDescription { get; set; }
        public string ApartmentCity { get; set; }
        public string ApartmentAddress { get; set; }
        public decimal ApartmentPrice { get; set; }
        public string ApartmentGender { get; set; }
        public int ApartmentUsers { get; set; }
        public int ApartmentRooms { get; set; }
        public string ApartmentCoverImage { get; set; }
        public ICollection<string> ApartmentPics { get; set; }
        public string ApartmentVideo { get; set; }
    }
}
