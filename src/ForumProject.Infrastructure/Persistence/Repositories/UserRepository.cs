using System.Runtime.CompilerServices;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Domain.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContextDapper _context;

        public UserRepository(DataContextDapper context)
        {
            _context = context;
        }
        public async Task AddFriendRequestAsync(User sender, User recipient)
        {
            using var connection = _context.CreateConnection();
            // const string sql = "";
            // return await connection.QuerySingleOrDefaultAsync<User>(
            // sql,
            // new { Id = id });
            throw new NotImplementedException();
        }

        public Task AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFriendRequestAsync(FriendRequest friendRequest)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFriendRequestByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserByEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FriendRequest>> GetFriendRequestsReceivedAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FriendRequest>> GetFriendRequestsSentAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByEmailAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByUsernameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}