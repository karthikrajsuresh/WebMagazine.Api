namespace WebMagazine.Api.Models
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageBase64 { get; set; }
        public int AuthorID { get; set; }
        public User Author { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsApproved { get; set; } = false;
        public int ViewCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }

}
