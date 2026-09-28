using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private int CurrentUserId =>
            int.TryParse(this.User.FindFirst("userId")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            return await _userService.GetAll();
        }

        [HttpGet("{userId:int}")]
        public async Task<ActionResult<UserDto>> GetUser(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if(user == null) return NotFound();
            return user;
        }

        [HttpGet("by-email")]
        public async Task<ActionResult<UserDto>> GetUserByEmail([FromQuery] string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            if(user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("by-username/{username}/")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _userService.GetByUsernameAsync(username);
            if(user == null) return NotFound();
            return Ok(user);
        }

        [HttpGet("me/friends")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserFriends()
        {
            var friends = await _userService.GetFriendsByIdAsync(CurrentUserId);
            return Ok(friends);
        }

        [HttpPatch("me/status")]
        public async Task<IActionResult> ActivateUser([FromBody] bool isActive)
        {
            var result = await _userService.UpdateStatusAsync(
                new UpdateStatusDto
                {
                    UserId = CurrentUserId,
                    IsActive = isActive
                }
            );
            return result ? NoContent() : BadRequest("Failed to update status");
        }

        [HttpPatch("me/email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailRequest request)
        {   
            var result = await _userService.UpdateEmailAsync(
                new UpdateEmailDto
                {
                    UserId = CurrentUserId,
                    Email = request.email
                }
            );
            return result ? NoContent() : BadRequest("Failed to update email.");
        }

        [HttpPatch("me/username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameRequest request)
        {
            var result = await _userService.UpdateUsernameAsync(
                new UpdateUsernameDto
                {
                    UserId = CurrentUserId,
                    Username = request.username
                }
            );
            return result ? NoContent() : BadRequest("Failed to update username.");
        }


        [HttpPut("me/friend-requests/{senderId:int}/accept")]
        public async Task<IActionResult> AcceptFriendRequest(int senderId)
        {
            var result = await _userService.AcceptFriendRequestAsync(
                new FriendRequestDto
                {
                    SenderId = senderId,
                    RecipientId = CurrentUserId,
                    Action = "remove"
                }
            );
            return result ? NoContent() : BadRequest("Failed to accept friend request.");
        }

        [HttpDelete("me/friends/{friendId:int}")]
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            var result = await _userService.DeleteFriendAsync(
                new UpdateFriendDto
                {
                    UserId = CurrentUserId,
                    FriendId = friendId
                }
            );
            return result ? NoContent() : BadRequest("Failed to remove friend.");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return result ? NoContent() : NotFound();
        }

        public record UpdateEmailRequest(string email);
        public record UpdateUsernameRequest(string username);
    }
}