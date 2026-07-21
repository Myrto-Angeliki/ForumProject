using ForumProject.Application.Features.Auths.DTOs;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Auths.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(RegistrationDto authDto);
        Task<bool> ChangePasswordAsync(LoginDto userForPasswordChange);
        Task<Dictionary<string, string>> LoginAsync(LoginDto userForLogin);
    }
}