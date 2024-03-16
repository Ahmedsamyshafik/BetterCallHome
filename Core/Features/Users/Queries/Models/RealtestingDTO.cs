using Microsoft.AspNetCore.Http;

namespace Core.Features.Users.Queries.Models
{
    public class RealtestingDTO
    {
        public IFormFile formFile { get; set; }
        public string nothing { get; set; }
        public string RequestScheme { get; set; }
        public HostString Requesthost { get; set; }
    }
}
