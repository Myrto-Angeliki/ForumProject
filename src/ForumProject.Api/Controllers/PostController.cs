using System.Security.Claims;
using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
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

        [AllowAnonymous]
        [HttpGet("posts")]
        public async Task<IEnumerable<PostDto>> GetPosts()
        {
            return await _postService.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("posts/{postId:int}")]
        public async Task<PostDto?> GetPost(int postId)
        {
            var response = await _postService.GetByIdAsync(postId);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("posts/by-user/{userId:int}")]
        public async Task<IEnumerable<PostDto>> GetUserPosts(int userId)
        {
            return await _postService.GetByUserAsync(userId);
        }

        [HttpGet("users/me/posts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetMyPosts()
        {
            var posts = await _postService.GetByUserAsync(CurrentUserId);
            return Ok(posts);
        }

        [HttpPost("posts")]
        public async Task<IActionResult> AddPost([FromBody] NewPostData data)
        {
            bool isAnyRowAffected = await _postService.AddAsync(new PostDto()
            {
                UserId = CurrentUserId,
                Title = data.Title,
                Content = data.Content,
                FeaturedImage = data.FeaturedImage
            });
            return Ok(isAnyRowAffected);
        }

        [HttpPut("posts/{postId:int}")]
        public async Task<IActionResult> EditPost([FromRoute] int postId,
            [FromBody] NewPostData data)
        {
            bool isAnyRowAffected = await _postService.UpdateAsync(new PostDto()
            {
                PostId = postId,
                UserId = CurrentUserId,
                Title = data.Title,
                Content = data.Content,
                FeaturedImage = data.FeaturedImage
            });
            return Ok(isAnyRowAffected);
        }

        [HttpDelete("users/me/posts/{postId:int}")]
        public async Task<IActionResult> DeletePost([FromRoute] int postId)
        {
            bool isAnyRowAffected = await _postService.DeleteAsync(new PostDto()
            {
                UserId = CurrentUserId,
                PostId = postId
            });
            if(isAnyRowAffected)
            {
                return Ok(isAnyRowAffected);
            }
            return NotFound(postId);
        }

        public record NewPostData(string Title, string Content, string FeaturedImage);
    }
}