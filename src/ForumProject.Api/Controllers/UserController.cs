using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFriendRequestService _friendRequestrService;

        public UserController(IUserService userService, IFriendRequestService friendRequestrService)
        {
            _userService = userService;
            _friendRequestrService = friendRequestrService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            return await _userService.GetAll();
        }

        [HttpGet("{userId:int}")]
        public async Task<UserDto> GetUser(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            return await _userService.GetByIdAsync(userId);
        }

        [HttpGet("by-email/{email}")]
        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await _userService.GetByEmailAsync(email);
        }

        [HttpGet("get-by-username/{username}/")]
        public async Task<UserDto> GetUserByUsername(string username)
        {
            return await _userService.GetByUsernameAsync(username);
        }

        [HttpGet("{userId}/friends")]
        public async Task<IEnumerable<UserDto>> GetUserFriends(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            return await _userService.GetFriendsByIdAsync(userId);
        }

        [HttpPut("{userId}/status")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            UpdateUserDto userToActivate = new UpdateUserDto
            {
                UserId = userId,
                IsActive = true,
                DeactivatedAt = null
            };
            
            bool result = await _userService.UpdateUserAsync(userToActivate);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to deactivate user!");
            return Ok();
        }

        [HttpPut("/deactivate-user/{userId}/")]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            UpdateUserDto userToDeactivate = new UpdateUserDto
            {
                UserId = userId,
                IsActive = false,
                DeactivatedAt = DateTime.UtcNow
            };
            
            bool result = await _userService.UpdateUserAsync(userToDeactivate);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to deactivate user!");
            return Ok();
        }

        [HttpPut("/update-email/{userId}/{email}")]
        public async Task<IActionResult> UpdateEmail(int userId, string email)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            UpdateUserDto updateEmail = new UpdateUserDto
            {
                UserId = userId,
                Email = email
            };
            bool result = await _userService.UpdateUserAsync(updateEmail);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to update email!");
            return Ok();
        }

        [HttpPut("/update-username/{userId}/{username}")]
        public async Task<IActionResult> UpdateUsername(int userId, string username)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            UpdateUserDto updateUsername = new UpdateUserDto
            {
                UserId = userId,
                Username = username
            };
            bool result = await _userService.UpdateUserAsync(updateUsername);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to update username!");
            return Ok();
        }

        [HttpPost("{senderId:int}/friend-requests/{recipientId:int}")]
        public async Task<IActionResult> SendFriendRequest(int senderId, int recipientId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            FriendRequestDto friendRequestDto = new FriendRequestDto
            {
                SenderId = senderId,
                RecipientId = recipientId,
                Action = "add"
            };
            bool result = await _friendRequestrService.AddAsync(friendRequestDto);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to send friend request!");
            return Ok();
        }

        [HttpPut("accept-friend-request")]
        public async Task<IActionResult> AcceptFriendRequest(UpdateFriendDto addFriendDto)
        {
            bool result = await _userService.AddFriendAsync(addFriendDto);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to accept friend request!");
            return Ok();
        }

        [HttpPut("remove-friend")]
        public async Task<IActionResult> RemoveFriend(UpdateFriendDto removeFriendDto)
        {
            bool result = await _userService.DeleteFriendAsync(removeFriendDto);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to remove friend!");
            return Ok();
        }

        [HttpDelete("/delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            bool result = await _userService.DeleteUserAsync(userId);
            // if(result)
            //     return Ok();
            // throw new Exception("Failed to delete user!");
            return Ok();
        }
    }
}