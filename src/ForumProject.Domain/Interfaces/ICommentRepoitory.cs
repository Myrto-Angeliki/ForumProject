using ForumProject.Domain.Entities;

namespace ForumProject.Domain.Interfaces
{
    public interface ICommentRepository : IDisposable
    {
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task AddCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);
        Task DeleteCommentByIdAsync(int commentId);
        Task DeleteCommentsByUser(User user);
        Task DeleteCommentsByPost(Post post);
    }
}