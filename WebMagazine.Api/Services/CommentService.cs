using Microsoft.EntityFrameworkCore;
using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;

namespace WebMagazine.Api.Services
{
    public class CommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.Article)
                .Include(c => c.ChildComments)
                .ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.Article)
                .Include(c => c.ChildComments)
                .FirstOrDefaultAsync(c => c.CommentID == id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> AddCommentAsync(CommentDTO commentDto)
        {
            var comment = new Comment
            {
                Content = commentDto.Content,
                ArticleID = commentDto.ArticleID,
                UserID = commentDto.UserID,
                CreatedAt = DateTime.UtcNow,
                ParentCommentID = commentDto.ParentCommentID
            };

            if (commentDto.ParentCommentID.HasValue)
            {
                var parentComment = await _context.Comments
                    .Include(c => c.ChildComments)
                    .FirstOrDefaultAsync(c => c.CommentID == commentDto.ParentCommentID.Value);
                if (parentComment != null)
                {
                    parentComment.ChildComments.Add(comment);
                }
            }

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.ChildComments)
                .FirstOrDefaultAsync(c => c.CommentID == id);

            if (comment != null)
            {
                if (comment.ChildComments.Any())
                {
                    _context.Comments.RemoveRange(comment.ChildComments);
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
