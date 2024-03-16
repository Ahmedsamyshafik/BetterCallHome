using Domin.Models;

namespace Core.Features.Apartments.Queries.Results
{
    public class PendingApartmentsResponse
    {
        public ICollection<PendingCollection> PendingApartments { get; set; }

    }
    public class PendingCollection
    {
        public string OwnerName { get; set; }
        public string? OwnerImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ApartmentTitle { get; set; }
        public string ApartmentCoverImage { get; set; }
        public string? Address { get; set; }
        public int NumberOfUsers { get; set; }
        public int Salary { get; set; }
    }
    public class TempMappingApartmentsOwnersToPendint
    {
        public ApplicationUser user { get; set; }
        public Apartment Apartment { get; set; }
    }
}
