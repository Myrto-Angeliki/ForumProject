using System.Data;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly DataContextDapper _context;

        public FriendshipRepository(DataContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(int userId1, int userId2)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spUser_InsertFriend"
                                            , new {@FriendshipMember1Id = userId1
                                                    , @FriendshipMember2Id = userId2}
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int userId1, int userId2)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spUser_DeleteFriend"
                                            , new { FriendId1 = userId1
                                                    , @FriendId2 = userId2}
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteByUserAsync(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                int rowsAffected = await connection.ExecuteAsync("ForumAppSchema.spUser_DeleteFriend"
                                            , new {FriendId1 = userId}
                                            , commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<User> friendShipMemebers = await connection.QueryAsync<User>(
                    "ForumAppSchema.spUser_GetFriends", commandType: CommandType.StoredProcedure
                );
                return friendShipMemebers;
            }
        }

        public async Task<IEnumerable<User>> GetByUserAsync(int userId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<User> friendShipMemebers = await connection.QueryAsync<User>(
                    "ForumAppSchema.spUser_GetFriends"
                    , new {UserId = userId}
                    , commandType: CommandType.StoredProcedure
                );
                return friendShipMemebers;
            }
        }
    }
}