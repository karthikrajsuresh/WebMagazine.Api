using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;

namespace WebMagazine.Api.IRepositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article> GetByIdAsync(int id);
        Task<Article> CreateArticleAsync(ArticleDTO articleDto);
        Task<Article> UpdateArticleAsync(int articleId, ArticleDTO articleDto);
        Task<string> UploadImageAsync(int articleId, string base64Image);
        Task DeleteAsync(int id);
    }
}
