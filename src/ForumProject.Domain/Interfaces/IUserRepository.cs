using ForumProject.Domain.Entities;

namespace ForumProject.Domain.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string userEmail);
        Task<User> GetUserByUsernameAsync(string userName);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task DeleteUserByIdAsync(int userId);
        Task DeleteUserByEmailAsync(string userEmail);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsSentAsync(User user);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsReceivedAsync(User user);
        Task AddFriendRequestAsync(User sender, User recipient);
        Task DeleteFriendRequestAsync(FriendRequest friendRequest);
        Task DeleteFriendRequestByUserIdAsync(int userId);
    }
}