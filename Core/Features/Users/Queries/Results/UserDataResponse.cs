namespace Core.Features.Users.Queries.Results
{
    public class UserDataResponse
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string? phone { get; set; }
        public string? College { get; set; }
        public string? University { get; set; }
        public string Gender { get; set; }
        //public string? Message { get; set; }
        public string Roles { get; set; }
        public string imagePath { get; set; }
        public string Email { get; set; }
        public int Views { get; set; }
        public int Apartments { get; set; }
    }
}
