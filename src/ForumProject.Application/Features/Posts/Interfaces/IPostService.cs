using ForumProject.Application.Features.Posts.DTOs;

namespace ForumProject.Application.Features.Posts.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllAsync();
        Task<PostDto> GetByIdAsync(int postId);
        Task<IEnumerable<PostDto>> GetByUserAsync(int userId);
        Task<bool> AddAsync(PostDto postDto);
        Task<bool> UpdateAsync(PostDto postDto);
        Task<bool> DeleteAsync(int postId);

    }
}