namespace Core.Features.Users.Queries.Results
{
    public class UserDataResponse
    {
        public string UserName { get; set; }
        public string? phone { get; set; }
        public string? College { get; set; }
        public string? University { get; set; }
        public string Gender { get; set; }
        public string? Message { get; set; }
        public string Roles { get; set; }
    }
}
