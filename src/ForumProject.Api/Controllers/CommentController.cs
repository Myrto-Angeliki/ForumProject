using System.Security.Claims;
using ForumProject.Application.Features.Comments.DTOs;
using ForumProject.Application.Features.Comments.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        private int CurrentUserId =>
            int.TryParse(this.User.FindFirst("userId")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("posts/{postId:int}/comments/{commentId:int}")]
        public async Task<CommentDto> GetComment(int commentId)
        {
            return await _commentService.GetById(commentId);
        }

        [AllowAnonymous]
        [HttpGet("posts/{postId:int}/comments")]
        public async Task<IEnumerable<CommentDto>> GetComments(int postId)
        {
            return await _commentService.GetByPost(postId);
        }

        [HttpGet("users/me/comments")]
        public async Task<IEnumerable<CommentDto>> GetMyComments()
        {
            int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");

            return await _commentService.GetByUser(userId);
        }

        [HttpPost("posts/{postId:int}/comments")]
        public async Task<IActionResult> AddComment(
            [FromRoute] CommentPostIdRequest requestInfo, 
            [FromBody] CommentContentRequest requestContent)
        {
            bool isAnyRowAffected = await _commentService.AddAsync(new AddCommentDto()
            {
                PostId = requestInfo.PostId,
                UserId = CurrentUserId,
                Content = requestContent.Content
            });
            return Ok(isAnyRowAffected);
        }

        [HttpPut("posts/{postId:int}/comments/{commentId:int}")]
        public async Task<IActionResult> UpdateComment(
            [FromRoute] CommentInfoRequest requestInfo, 
            [FromBody] CommentContentRequest requestContent
        )
        {
            bool isAnyRowAffected = await _commentService.UpdateAsync(new CommentDto()
            {
                CommentId = requestInfo.CommentId,
                PostId = requestInfo.PostId,
                UserId = CurrentUserId,
                Content = requestContent.Content
            });
            return Ok(isAnyRowAffected);
        }

        private async Task<IActionResult> DeleteComment(int commentId, bool fromUserCommentList)
        {
            bool isAnyRowAffected = await _commentService.DeleteAsync(new CommentDto()
            {
                CommentId = commentId,
                UserId = CurrentUserId
            }, fromUserCommentList);
            if(isAnyRowAffected)
                return Ok();
            return NotFound();
        }

        [HttpDelete("posts/{postId:int}/comments/{commentId}")]
        public async Task<IActionResult> DeleteCommentUnderPost(int commentId)
        {
            return await DeleteComment(commentId, fromUserCommentList: false);
        }

        [HttpDelete("users/me/comments/{commentId}")]
        public async Task<IActionResult> DeleteCommentFromUserComments(int commentId)
        {
            return await DeleteComment(commentId, fromUserCommentList: true);
        }
    }

    public record CommentContentRequest(string Content);
    public record CommentInfoRequest(int CommentId, int PostId);
    public record CommentPostIdRequest(int PostId);
}