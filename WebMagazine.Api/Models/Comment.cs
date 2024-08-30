namespace WebMagazine.Api.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ArticleID { get; set; }
        public Article Article { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
