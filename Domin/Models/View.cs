namespace Domin.Models
{
    public class View
    {

        public int Id { get; set; }
        public int count { get; set; }
        // [ForeignKey("Users")]
        public string ViewerID { get; set; }
        // [ForeignKey("User")]
        public string ViewedID { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        //  public ApplicationUser? User { get; set; }
        //public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
