namespace WebMagazine.Api.Models
{
    public class Subscription
    {
        public int SubscriptionID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}