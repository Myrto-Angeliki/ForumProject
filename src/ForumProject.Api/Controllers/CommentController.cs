using ForumProject.Application.Features.Comments.DTOs;
using ForumProject.Application.Features.Comments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [ApiController]
    [Route("/")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("posts/post-id={postId}/comments")]
        public async Task<IEnumerable<CommentDto>> GetComments(int postId = 0)
        {
            return await _commentService.GetByPost(postId);
        }

        [HttpGet("user/my-comments")]
        public async Task<IEnumerable<CommentDto>> GetMyComments()
        {
            int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");

            return await _commentService.GetByUser(userId);
        }

        [HttpPut("user/upsert-comment")]
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
            return Ok();
        }

        [HttpDelete("user/comment-id={commentId}")]
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