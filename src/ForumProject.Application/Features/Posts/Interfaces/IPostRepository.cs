using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post?> GetByIdAsync(int postId);
        Task<IEnumerable<Post>> GetByUserAsync(int userId);
        Task<bool> AddAsync(Post post);
        Task<bool> UpdateAsync(Post post);
        Task<bool> DeleteAsync(int postId);
    }
}