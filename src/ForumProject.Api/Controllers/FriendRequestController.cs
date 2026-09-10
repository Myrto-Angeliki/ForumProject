using System.Security.Claims;
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

        // [HttpGet("requests-sent")]
        // public async Task<ActionResult<IEnumerable<FriendRequest>>> GetFriendRequestsSent()
        // {
        //     var requestsSent = await _friendRequestService.GetBySenderIdAsync(CurrentUserId);
        //     return Ok(requestsSent);
        // }

        // [HttpGet("requests-received")]
        // public async Task<ActionResult<IEnumerable<FriendRequest>>> GetFriendRequestsReceived()
        // {
        //     var requestsReceived = await _friendRequestService.GetByRecipientIdAsync(CurrentUserId);
        //     return Ok(requestsReceived);
        // }
    }
}