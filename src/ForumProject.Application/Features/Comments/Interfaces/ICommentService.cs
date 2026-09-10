
using ForumProject.Application.Features.Comments.DTOs;

namespace ForumProject.Application.Features.Comments.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> GetById(int commentId);
        Task<IEnumerable<CommentDto>> GetByUser(int userId);
        Task<IEnumerable<CommentDto>> GetByPost(int postId);
        Task<bool> AddAsync(AddCommentDto commentDto);
        Task<bool> DeleteAsync(int commentId);
        Task<bool> UpdateAsync(CommentDto commentDto);
    }
}