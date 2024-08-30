namespace WebMagazine.Api.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
