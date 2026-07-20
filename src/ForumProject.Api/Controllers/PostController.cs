using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [ApiController]
    [Route("/")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("posts")]
        public async Task<IEnumerable<PostDto>> GetPosts()
        {
            return await _postService.GetAllAsync();
        }

        [HttpGet("posts/post-id={postId}")]
        public async Task<PostDto?> GetPost(int postId)
        {
            var response = await _postService.GetByIdAsync(postId);
            if(response == null)
            {
                throw new Exception("There is no post with id = " + postId.ToString());
            }
            return response;
        }

        [HttpGet("posts/user-id={userId}")]
        public async Task<IEnumerable<PostDto>> GetUserPosts(int userId)
        {
            return await _postService.GetByUserAsync(userId);
        }

        [HttpGet("user/my-posts")]
        public async Task<IEnumerable<PostDto>> GetMyPosts()
        {
            int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");

            return await _postService.GetByUserAsync(userId);
        }

        [HttpPost("user/add-post")]
        public async Task<IActionResult> AddPost(PostDto postDto)
        {
            bool isAnyRowAffected = await _postService.AddAsync(postDto);
            
            if(isAnyRowAffected)
            {
                return Ok();
            }
            throw new Exception("Failed to Add Post!");
        }

        [HttpPut("user/edit-post")]
        public async Task<IActionResult> EditPost(PostDto postDto)
        {
            bool isAnyRowAffected = await _postService.UpdateAsync(postDto);

            if(isAnyRowAffected)
            {
                return Ok();
            }
            throw new Exception("Failed to Update Post!");
        }

        [HttpDelete("user/post-id={postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            bool isAnyRowAffected = await _postService.DeleteAsync(postId);
            if(isAnyRowAffected)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}