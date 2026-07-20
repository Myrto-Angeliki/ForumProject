using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Interfaces;
using ForumProject.Application.Features.Posts.Mappers;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Posts.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public  PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> AddAsync(PostDto postDto)
        {
            Post post = PostServiceHelper.GetPostToUpsert(postDto, isInsert: true);
            return await _postRepository.AddAsync(post);
        }

        public async Task<bool> DeleteAsync(int postId)
        {
            return await _postRepository.DeleteAsync(postId);
        }

        public async Task<IEnumerable<PostDto>> GetAllAsync()
        {
            IEnumerable<Post> posts = await _postRepository.GetAllAsync();
            return PostMapper.MapToPostDtos(posts);
        }

        public async Task<PostDto?> GetByIdAsync(int postId)
        {
            Post? post = await _postRepository.GetByIdAsync(postId);
            if(post != null)
            {
                return PostMapper.MapToPostDto(post);
            }
            return null;
        }

        public async Task<IEnumerable<PostDto>> GetByUserAsync(int userId)
        {
            IEnumerable<Post> posts = await _postRepository.GetByUserAsync(userId);
            return PostMapper.MapToPostDtos(posts);
        }

        public async Task<bool> UpdateAsync(PostDto postDto)
        {
            Post post = PostServiceHelper.GetPostToUpsert(postDto, isInsert: false);
            return await _postRepository.UpdateAsync(post);
        }
    }
}