using System.Security.Claims;
using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        private int CurrentUserId =>
            int.TryParse(this.User.FindFirst("userId")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;

        [HttpGet]
        public async Task<IEnumerable<PostDto>> GetPosts()
        {
            return await _postService.GetAllAsync();
        }

        [HttpGet("{postId:int}")]
        public async Task<PostDto?> GetPost(int postId)
        {
            var response = await _postService.GetByIdAsync(postId);
            return response;
        }

        [HttpGet("by-user/{userId:int}")]
        public async Task<IEnumerable<PostDto>> GetUserPosts(int userId)
        {
            return await _postService.GetByUserAsync(userId);
        }

        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetMyPosts()
        {
            var posts = await _postService.GetByUserAsync(CurrentUserId);
            return Ok(posts);
        }

        [HttpPost()]
        public async Task<IActionResult> AddPost(PostDto postDto)
        {
            bool isAnyRowAffected = await _postService.AddAsync(postDto);
            return Ok();
        }

        [HttpPut()]
        public async Task<IActionResult> EditPost(PostDto postDto)
        {
            bool isAnyRowAffected = await _postService.UpdateAsync(postDto);
            return Ok();
        }

        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            bool isAnyRowAffected = await _postService.DeleteAsync(postId);
            if(isAnyRowAffected)
            {
                return Ok();
            }
            return NotFound(postId);
        }
    }
}