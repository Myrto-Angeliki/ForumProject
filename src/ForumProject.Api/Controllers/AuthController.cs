using System.Security.Claims;
using ForumProject.Application.Features.Auths.DTOs;
using ForumProject.Application.Features.Auths.Interfaces;
using ForumProject.Application.Features.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumProject.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto registrationDto)
        {
            bool isAnyRowChanged = await _authService.RegisterUserAsync(registrationDto);
            return Ok(isAnyRowChanged);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto userForLogin)
        {
            Dictionary<string, string> dict = await _authService.LoginAsync(userForLogin);
            return Ok(dict);
        }

        [HttpGet("token")]
        public async Task<IActionResult> RefreshToken()
        {
            int currentUserId =
            int.TryParse(this.User.FindFirst("userId")?.Value 
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;

            string token = await _authService.RefreshTokenAsync(currentUserId);
            return Ok(token);
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePasswword(LoginDto userForPasswordChange)
        {
            bool isAnyRowChanged = await _authService.ChangePasswordAsync(userForPasswordChange);
            return Ok();
        }
    }
}