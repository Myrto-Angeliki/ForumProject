using ForumProject.Domain.Entities;

namespace ForumProject.Domain.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(int userId);
        Task<Post?> GetPostByIdAsync(int postId);
        Task<Post?> GetPostByPostTitleAsync(string postTitle);
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(Post post);
        Task DeletePostByIdAsync(int postId);
        Task DeletePostsByUser(User user);
    }
}