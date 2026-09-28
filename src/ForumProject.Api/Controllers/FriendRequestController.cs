using System.Security.Claims;
using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
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


        [HttpPut("users/me/friend-requests/{senderId:int}/accept")]
        public async Task<IActionResult> AcceptFriendRequest(int senderId)
        {
            var result = await _friendRequestService.AcceptFriendRequestAsync(
                new FriendRequestDto
                {
                    SenderId = senderId,
                    RecipientId = CurrentUserId
                }
            );
            return result ? NoContent() : BadRequest("Failed to accept friend request.");
        }

        [HttpDelete("users/me/friend-requests/{senderId:int}/deny")]
        public async Task<IActionResult> DenyFriendRequest(int senderId)
        {
            var result = await _friendRequestService.DeleteAsync(
                new FriendRequestDto
                {
                    SenderId = senderId,
                    RecipientId = CurrentUserId
                }
            );
            return result ? NoContent() : BadRequest("Failed to deny friend request.");
        }

        [HttpPost("users/me/friend-requests/{recipientId:int}")]
        public async Task<IActionResult> SendFriendRequest(int recipientId)
        {
            var result = await _friendRequestService.AddAsync(
                new FriendRequestDto
                {
                    SenderId = CurrentUserId,
                    RecipientId = recipientId,
                }
            );
            return result ? NoContent() : BadRequest("Failed to send friend request.");
        }

        [HttpDelete("users/me/friend-requests/{recipientId:int}")]
        public async Task<IActionResult> DeleteFriendRequest(int recipientId)
        {
            var result = await _friendRequestService.DeleteAsync(
                new FriendRequestDto
                {
                    SenderId = CurrentUserId,
                    RecipientId = recipientId
                }
            );
            return result ? NoContent() : BadRequest("Failed to delete friend request.");
        }
    }
}