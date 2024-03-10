namespace Domin.Models
{
    public class NotificationTransaction
    {
        public int Id { get; set; }
        public string? userID { get; set; }
        public int CommentId { get; set; }
        public int ReactId { get; set; }
    }
}
