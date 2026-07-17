using ForumProject.Application.Features.Comments.DTOs;
using ForumProject.Application.Features.Comments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("Comments/{postId}")]
        public async Task<IEnumerable<CommentDto>> GetComments(int postId = 0)
        {
            return await _commentService.GetByPost(postId);
        }

        [HttpGet("MyComments")]
        public async Task<IEnumerable<CommentDto>> GetMyComments()
        {
            int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");

            return await _commentService.GetByUser(userId);
        }

        [HttpPut("UpsertComment")]
        public async Task<IActionResult> UpsertComment(CommentDto commentDto)
        {
            bool isAnyRowAffected;
            if(commentDto.CommentId != 0)
            {
                isAnyRowAffected = await _commentService.UpdateAsync(commentDto);
            }
            else
            {
                isAnyRowAffected = await _commentService.AddAsync(commentDto);
            }
            if(isAnyRowAffected)
            {
                return Ok();
            }
            throw new Exception("Failed to Upsert Comment!");
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            bool isAnyRowAffected = await _commentService.DeleteAsync(commentId);
            if(isAnyRowAffected)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}