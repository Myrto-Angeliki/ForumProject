using ForumProject.Application.Features.Auths.DTOs;
using ForumProject.Application.Features.Auths.Interfaces;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto registrationDto)
        {
            bool isAnyRowChanged = await _authService.RegisterUserAsync(registrationDto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto userForLogin)
        {
            Dictionary<string, string> dict = await _authService.LoginAsync(userForLogin);
            return Ok(dict);
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePasswword(LoginDto userForPasswordChange)
        {
            bool isAnyRowChanged = await _authService.ChangePasswordAsync(userForPasswordChange);
            return Ok();
        }
    }
}