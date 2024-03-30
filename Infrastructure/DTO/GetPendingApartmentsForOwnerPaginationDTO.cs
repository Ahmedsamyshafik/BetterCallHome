namespace Infrastructure.DTO
{
    public class GetPendingApartmentsForOwnerPaginationDTO
    {
        public int ApartmentID { get; set; }
        public string ApartmentTitle { get; set; }
        public string Description { get; set; }
        public string ApartmentCoverImage { get; set; }
        public string? Address { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal Salary { get; set; }
        public DateTime? PublishedAt { get; set; }

    }
}
