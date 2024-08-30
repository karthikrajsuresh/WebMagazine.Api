namespace WebMagazine.Api.DTOs
{
    public class CommentDTO
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ArticleID { get; set; }
        public int UserID { get; set; }
    }
}
