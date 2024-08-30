namespace WebMagazine.Api.Models
{
    public class Tag
    {
        public int TagID { get; set; }
        public string Name { get; set; }

        public ICollection<ArticleTag> ArticlesTags { get; set; }
    }
}