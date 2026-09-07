using AutoMapper;
using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.Comments.DTOs;
using ForumProject.Application.Features.Comments.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Comments.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public  CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
            _mapper = new Mapper(new MapperConfiguration((cfg) =>
            {
                cfg.CreateMap<CommentDto, Comment>();
                cfg.CreateMap<Comment, CommentDto>();
            }));
        }

        public async Task<bool> AddAsync(CommentDto commentDto)
        {
            return await _commentRepository.AddAsync(_mapper.Map<Comment>(commentDto));
        }

        public async Task<bool> DeleteAsync(int commentId)
        {
            return await _commentRepository.DeleteAsync(commentId);
        }

        public async Task<CommentDto> GetById(int commentId)
        {
            Comment comment = await _commentRepository.GetByIdAsync(commentId)
                ?? throw new NotFoundException(nameof(Comment), commentId);
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetByPost(int postId)
        {
            var comments = await _commentRepository.GetByPostAsync(postId);
            return comments.Select(_mapper.Map<Comment, CommentDto>);
        }

        public async Task<IEnumerable<CommentDto>> GetByUser(int userId)
        {
            var comments = await _commentRepository.GetByUserAsync(userId);
            return comments.Select(_mapper.Map<Comment, CommentDto>);
        }

        public async Task<bool> UpdateAsync(CommentDto commentDto)
        {
            return await _commentRepository.UpdateAsync(_mapper.Map<Comment>(commentDto));
        }
    }
}