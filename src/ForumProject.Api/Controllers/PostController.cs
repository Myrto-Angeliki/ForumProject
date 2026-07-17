using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("Posts")]
        public async Task<IEnumerable<PostDto>> GetPosts()
        {
            return await _postService.GetAllAsync();
        }

        [HttpGet("Posts/{userId}")]
        public async Task<IEnumerable<PostDto>> GetUserPosts(int userId)
        {
            return await _postService.GetByUser(userId);
        }

        [HttpGet("MyPosts")]
        public async Task<IEnumerable<PostDto>> GetMyComments()
        {
            int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");

            return await _postService.GetByUser(userId);
        }
    }
}