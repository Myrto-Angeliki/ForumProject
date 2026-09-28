using ForumProject.Application.Features.Friendships.DTOs;
using ForumProject.Application.Features.Users.DTOs;

namespace ForumProject.Application.Features.Friendships.Interfaces;

public interface IFriendshipService
{
    Task<IEnumerable<UserDto>> GetFriendsByIdAsync(int userId);
    Task<bool> CreateFriendship(UpdateFriendDto dto);
    Task<bool> DeleteFriendship(UpdateFriendDto dto);
}