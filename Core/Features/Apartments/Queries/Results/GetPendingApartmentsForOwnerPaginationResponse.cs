namespace Core.Features.Apartments.Queries.Results
{
    public class GetPendingApartmentsForOwnerPaginationResponse
    {

        public string ApartmentTitle { get; set; }

        public int ApartmentID { get; set; }
        public string Description { get; set; }
        public string ApartmentCoverImage { get; set; }
        public string? Address { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal Salary { get; set; }
    }
}
