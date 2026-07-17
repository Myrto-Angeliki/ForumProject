
using ForumProject.Application.Features.Comments.DTOs;

namespace ForumProject.Application.Features.Comments.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetByUser(int userId);
        Task<IEnumerable<CommentDto>> GetByPost(int postId);
        Task<bool> AddAsync(CommentDto commentDto);
        Task<bool> DeleteAsync(int commentId);
        Task<bool> UpdateAsync(CommentDto commentDto);
    }
}