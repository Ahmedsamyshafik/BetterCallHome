using Domin.Models;

namespace Infrastructure.DTO
{
    public class ReturnedUserDataDto
    {
        public string Message { get; set; }
        public ApplicationUser? user { get; set; }
        public int OperationCount { get; set; }
    }
}
