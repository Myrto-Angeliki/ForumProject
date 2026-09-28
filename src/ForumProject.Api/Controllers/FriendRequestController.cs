using System.Security.Claims;
using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/friend-requests")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        private int CurrentUserId =>
            int.TryParse(this.User.FindFirst("userId")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;

        [HttpPost("me/{recipientId:int}")]
        public async Task<IActionResult> SendFriendRequest(int recipientId)
        {
            var result = await _friendRequestService.AddAsync(
                new FriendRequestDto
                {
                    SenderId = CurrentUserId,
                    RecipientId = recipientId,
                    Action = "add"
                }
            );
            return result ? NoContent() : BadRequest("Failed to send friend request.");
        }
    }
}