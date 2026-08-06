using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-all")]
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            return await _userService.GetAll();
        }

        [HttpGet("get-by-id/{userId}/")]
        public async Task<UserDto> GetUser(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            return await _userService.GetByIdAsync(userId);
        }

        [HttpGet("get-by-email/{email}/")]
        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await _userService.GetByEmailAsync(email);
        }

        [HttpGet("get-by-username/{username}/")]
        public async Task<UserDto> GetUserByUsername(string username)
        {
            return await _userService.GetByUsernameAsync(username);
        }

        [HttpGet("get-friends/{userId}/")]
        public async Task<IEnumerable<UserDto>> GetUserFriends(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            return await _userService.GetFriendsByIdAsync(userId);
        }

        [HttpPut("/activate-user/{userId}/")]
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
            if(result)
                return Ok();
            throw new Exception("Failed to deactivate user!");
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
            if(result)
                return Ok();
            throw new Exception("Failed to deactivate user!");
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
            if(result)
                return Ok();
            throw new Exception("Failed to update email!");
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
            if(result)
                return Ok();
            throw new Exception("Failed to update username!");
        }

        [HttpPut("/accept-friend-request")]
        public async Task<IActionResult> AcceptFriendRequest(UpdateFriendDto addFriendDto)
        {
            bool result = await _userService.AddFriendAsync(addFriendDto);
            if(result)
                return Ok();
            throw new Exception("Failed to accept friend request!");
        }

        [HttpPut("/remove-friend")]
        public async Task<IActionResult> RemoveFriend(UpdateFriendDto removeFriendDto)
        {
            bool result = await _userService.DeleteFriendAsync(removeFriendDto);
            if(result)
                return Ok();
            throw new Exception("Failed to remove friend!");
        }

        [HttpDelete("/delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            bool result = await _userService.DeleteUserAsync(userId);
            if(result)
                return Ok();
            throw new Exception("Failed to delete user!");
        }
    }
}