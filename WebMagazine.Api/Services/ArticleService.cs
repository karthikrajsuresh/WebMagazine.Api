using Microsoft.EntityFrameworkCore;
using WebMagazine.Api.Models;

namespace WebMagazine.Api.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.Include(a => a.Author).Include(a => a.Category).ToListAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.Include(a => a.Author).Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleID == id);
        }

        public async Task<Article> CreateAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<Article> UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<string> UploadImageAsync(int articleId, string base64Image)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null)
            {
                throw new ArgumentException("Article not found");
            }

            article.ImageBase64 = base64Image;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return base64Image;
        }

        public async Task<string> GetImageAsBase64StringAsync(int articleId)
        {
            var article = await _context.Articles.FindAsync(articleId);
            if (article == null || string.IsNullOrEmpty(article.ImageBase64))
            {
                throw new ArgumentException("Image not found");
            }

            return article.ImageBase64;
        }

        public async Task<string> ConvertImageToBase64Async(string imagePath)
        {
            var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] array = new byte[fs.Length];
            await fs.ReadAsync(array);
            return Convert.ToBase64String(array);
        }

        public async Task SaveBase64AsImageAsync(string base64Image, string savePath)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Image);
            await File.WriteAllBytesAsync(savePath, imageBytes);
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }
    }

}
