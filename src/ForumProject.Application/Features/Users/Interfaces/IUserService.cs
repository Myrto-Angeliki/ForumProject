using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Users.Interfaces
{
    public interface IUserService
    {
        Task<bool> UpdateUser(UpdateUserDto userDto);
        Task<bool> DeleteUser(int userId);
        Task<IEnumerable<User>> GetFriendsByIdAsync(int userId);
        Task<bool> AddFriend(UpdateFriendDto addFriendDto);
        Task<bool> DeleteFriendAsync(UpdateFriendDto removeFriendDto);
    }
}