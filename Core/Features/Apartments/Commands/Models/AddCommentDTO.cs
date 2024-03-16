namespace Core.Features.Apartments.Commands.Models
{
    public class AddCommentDTO
    {
        public string UserID { get; set; }
        public int ApartmentID { get; set; }
        public string Comment { get; set; }
    }
}
