using ForumProject.Application.Features.Comments.DTOs;
using ForumProject.Application.Features.Comments.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [AllowAnonymous]
        [HttpGet("by-post/{postId:int}")]
        public async Task<IEnumerable<CommentDto>> GetComments(int postId)
        {
            return await _commentService.GetByPost(postId);
        }

        [HttpGet("me")]
        public async Task<IEnumerable<CommentDto>> GetMyComments()
        {
            int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");

            return await _commentService.GetByUser(userId);
        }

        [HttpPost()]
        public async Task<IActionResult> UpsertComment(AddCommentDto dto)
        {
            bool isAnyRowAffected = await _commentService.AddAsync(dto);
            return Ok();
        }

        [HttpPut()]
        public async Task<IActionResult> UpsertComment(CommentDto commentDto)
        {
            bool isAnyRowAffected = await _commentService.UpdateAsync(commentDto);
            return Ok();
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            bool isAnyRowAffected = await _commentService.DeleteAsync(commentId);
            if(isAnyRowAffected)
                return Ok();
            return NotFound();
        }
    }
}