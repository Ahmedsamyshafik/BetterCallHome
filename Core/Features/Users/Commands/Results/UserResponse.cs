namespace Core.Features.Users.Commands.Results
{
    public class UserResponse
    {

        public string UserName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string? College { get; set; }

        public string? University { get; set; }

        public string Gender { get; set; }

        public string Message { get; set; }

        public bool IsAuthenticated { get; set; }

        public List<string> Roles { get; set; }

        public string Token { get; set; }

        public DateTime Expier { get; set; }
    }
}
