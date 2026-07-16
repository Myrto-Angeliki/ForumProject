using ForumProject.Domain.Entities;

namespace ForumProject.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int commentId);
        Task<IEnumerable<Comment>> GetByUserAsync(int userId);
        Task<IEnumerable<Comment>> GetByPostAsync(int postId);
        Task<bool> AddAsync(Comment comment);
        Task<bool> UpdateAsync(Comment comment);
        Task<bool> DeleteAsync(int commentId);
    }
}