using AutoMapper;
using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public  PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _mapper = new Mapper(new MapperConfiguration((cfg) =>
            {
                cfg.CreateMap<Post, PostDto>();
                cfg.CreateMap<PostDto, Post>();
            }));
        }

        public async Task<bool> AddAsync(PostDto postDto)
        {
            Post post = _mapper.Map<Post>(postDto);
            PostServiceHelper.GetPostToUpsert(post);
            return await _postRepository.AddAsync(post);
        }

        public async Task<bool> DeleteAsync(PostDto dto)
        {
            var userPosts = await GetByUserAsync(dto.UserId);
            foreach(PostDto userPost in userPosts)
            {
                if(userPost.PostId == dto.PostId)
                    return await _postRepository.DeleteAsync(dto.PostId);
            }
            throw new UnauthorizedAccessException("Cannot delete the post of a different user!");
        }

        public async Task<IEnumerable<PostDto>> GetAllAsync()
        {
            IEnumerable<Post> posts = await _postRepository.GetAllAsync();
            return posts.Select(_mapper.Map<Post, PostDto>);
        }

        public async Task<PostDto> GetByIdAsync(int postId)
        {
            Post? post = await _postRepository.GetByIdAsync(postId);
            if(post != null)
                return _mapper.Map<PostDto>(post);
            throw new NotFoundException(nameof(post), postId);
        }

        public async Task<IEnumerable<PostDto>> GetByUserAsync(int userId)
        {
            IEnumerable<Post> posts = await _postRepository.GetByUserAsync(userId);
            return posts.Select(_mapper.Map<Post, PostDto>);
        }

        public async Task<bool> UpdateAsync(PostDto postDto)
        {
            Post post = _mapper.Map<Post>(postDto);
            PostServiceHelper.GetPostToUpsert(post);
            return await _postRepository.UpdateAsync(post);
        }
    }
}