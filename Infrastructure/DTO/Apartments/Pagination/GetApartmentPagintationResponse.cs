namespace infrustructure.DTO.Apartments.Pagination
{
    public class GetApartmentPagintationResponse
    {
        public int ApartmentID { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Salary { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int TotalCount { get; set; }
        public int ApartmentExistCount { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
