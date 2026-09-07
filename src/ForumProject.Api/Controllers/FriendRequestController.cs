using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("/friend-requests")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        // [HttpGet("get-all/")]
        // public async Task<IEnumerable<FriendRequestDto>> GetAllFriendRequests()
        // {
        //     //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
        //     return await _friendRequestService.GetAsync(new FriendRequestDto());
        // }

        [HttpGet("get-requests-sent/{userId}/")]
        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsSent(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            return await _friendRequestService.GetBySenderIdAsync(userId);
        }

        [HttpGet("get-requests-received/{userId}/")]
        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsReceived(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            return await _friendRequestService.GetByRecipientIdAsync(userId);
        }
    }
}