namespace WebMagazine.Api.Models
{
    public class ArticleTag
    {
        public int ArticleID { get; set; }
        public Article Article { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}
