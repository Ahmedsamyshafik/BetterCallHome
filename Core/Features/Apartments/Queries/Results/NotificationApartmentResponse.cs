namespace Core.Features.Apartments.Queries.Results
{
    public class NotificationApartmentResponse
    {
        public List<ReturnedNotify>? notifies { get; set; }
    }
    public class ReturnedNotify
    {
        public string Type { get; set; }
        public string? UserImage { get; set; }
        public string? UserName { get; set; }
        public DateTime? date { get; set; }
        public int OperationId { get; set; }
    }


}
