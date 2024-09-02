using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;

namespace WebMagazine.Api.IRepositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment> UpdateAsync(Comment comment);
        Task<Comment> AddCommentAsync(CommentDTO commentDto);
        Task DeleteAsync(int id);
    }
}