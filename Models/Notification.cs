namespace JobPortal.Models
{
    public class Notification
    {
        public int Id { get; set; }

        //[Required]
        public string Message { get; set; } = string.Empty;

        public string SenderId { get; set; }     // FK for emp
        public User Sender { get; set; }         // nav for emp
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }


}
