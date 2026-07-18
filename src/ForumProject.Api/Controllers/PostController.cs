using ForumProject.Application.Features.Posts.DTOs;
using ForumProject.Application.Features.Posts.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpGet("{postId}")]
        public async Task<PostDto?> GetPost(int postId)
        {
            var response = await _postService.GetByIdAsync(postId);
            if(response == null)
            {
                throw new Exception("There is no post with id = " + postId.ToString());
            }
            return response;
        }

        [HttpGet("Posts/{userId}")]
        public async Task<IEnumerable<PostDto>> GetUserPosts(int userId)
        {
            return await _postService.GetByUserAsync(userId);
        }

        [HttpGet("MyPosts")]
        public async Task<IEnumerable<PostDto>> GetMyPosts()
        {
            int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");

            return await _postService.GetByUserAsync(userId);
        }

        [HttpPut("UpsertComment")]
        public async Task<IActionResult> UpsertComment(PostDto postDto)
        {
            bool isAnyRowAffected;
            if(postDto.PostId != 0)
            {
                isAnyRowAffected = await _postService.UpdateAsync(postDto);
            }
            else
            {
                isAnyRowAffected = await _postService.AddAsync(postDto);
            }
            if(isAnyRowAffected)
            {
                return Ok();
            }
            throw new Exception("Failed to Upsert Post!");
        }

        [HttpDelete("{postId}")]
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