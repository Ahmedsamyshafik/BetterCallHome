namespace Infrastructure.DTO
{
    public class ApartmentPaginationPending
    {
        public string OwnerName { get; set; }
        public string? OwnerImage { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string ApartmentTitle { get; set; }
        //public string ApartmentCoverImage { get; set; }
        public string? Address { get; set; }
        public int NumberOfUsers { get; set; }
        public decimal Salary { get; set; }

        public List<string> ApartmentsPics { get; set; }


    }
}
