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

        public async Task<IEnumerable<UserDto>> GetUserFriends(int userId)
        {
            //int userId = Int32.Parse(this.User.FindFirst("userId")?.Value ?? "0");
            return await _userService.GetFriendsByIdAsync(userId);
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

        [HttpPut("/update-user")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            bool result = await _userService.UpdateUserAsync(updateUserDto);
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