namespace WebMagazine.Api.DTOs
{
    public class NotificationDTO
    {
        public int NotificationID { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public int UserID { get; set; }
    }

}
