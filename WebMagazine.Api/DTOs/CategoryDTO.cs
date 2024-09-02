namespace WebMagazine.Api.DTOs
{
    public class CategoryDTO
    {
        public int UserID { get; set; }
        public int? ParentCommentID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }
}