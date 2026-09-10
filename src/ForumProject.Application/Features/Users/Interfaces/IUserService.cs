using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.Users.DTOs;

namespace ForumProject.Application.Features.Users.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto> GetByIdAsync(int userId);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<bool> UpdateStatusAsync(UpdateStatusDto userDto);
        Task<bool> UpdateEmailAsync(UpdateEmailDto dto);
        Task<bool> UpdateUsernameAsync(UpdateUsernameDto userDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<IEnumerable<UserDto>> GetFriendsByIdAsync(int userId);
        Task<bool> AcceptFriendRequestAsync(FriendRequestDto friendRequestDto);
        Task<bool> DenyFriendRequestAsync(FriendRequestDto friendRequestDto);
        Task<bool> DeleteFriendAsync(UpdateFriendDto removeFriendDto);
    }
}