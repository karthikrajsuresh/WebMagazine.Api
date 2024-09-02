using System.ComponentModel.DataAnnotations;

namespace WebMagazine.Api.DTOs
{
    public class ArticleDTO
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }

        [MaxLength(10000, ErrorMessage = "Content length can't exceed 10,000 characters.")]
        public string Content { get; set; }
        public string? ImageBase64 { get; set; }
        public int AuthorID { get; set; }
        public int CategoryID { get; set; }
        public DateTime? PublishedAt { get; set; }
        public bool IsApproved { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
