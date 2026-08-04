using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Users.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto> GetByIdAsync(int userId);
        Task<bool> UpdateUserAsync(UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<IEnumerable<UserDto>> GetFriendsByIdAsync(int userId);
        Task<bool> AddFriendAsync(UpdateFriendDto addFriendDto);
        Task<bool> DeleteFriendAsync(UpdateFriendDto removeFriendDto);
    }
}