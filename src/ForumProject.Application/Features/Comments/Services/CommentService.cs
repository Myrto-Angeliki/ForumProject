using ForumProject.Application.Features.Comments.DTOs;
using ForumProject.Application.Features.Comments.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public  CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<bool> AddAsync(CommentDto commentDto)
        {
            Comment comment = commentDto.Map();
            return await _commentRepository.AddAsync(comment);
        }

        public async Task<bool> DeleteAsync(int commentId)
        {
            return await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<IEnumerable<CommentDto>> GetByPost(int postId)
        {
            var comments = await _commentRepository.GetByPostAsync(postId);
            return CommentDto.Map(comments);
        }

        public async Task<IEnumerable<CommentDto>> GetByUser(int userId)
        {
            var comments = await _commentRepository.GetByUserAsync(userId);
            return CommentDto.Map(comments);
        }

        public async Task<bool> UpdateAsync(CommentDto commentDto)
        {
            Comment comment = commentDto.Map();
            return await _commentRepository.UpdateAsync(comment);
        }
    }
}