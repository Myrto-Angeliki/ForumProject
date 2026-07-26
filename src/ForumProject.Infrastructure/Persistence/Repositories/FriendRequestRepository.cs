using System.Data;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Infrastructure.Persistence;

namespace ForumProject.Infrastructure.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly DataContextDapper _context;

        public FriendRequestRepository(DataContextDapper context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(int senderId, int recipientId)
        {
            using(var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(
                                    "ForumAppSchema.spFriendRequest_Insert"
                                    , new {SenderId = senderId, RecipientId = recipientId}
                                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int senderId, int recipientId)
        {
            using(var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(
                                    "ForumAppSchema.spFriendRequest_Delete"
                                    , new {SenderId = senderId, RecipientId = recipientId}
                                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteByRecipientIdAsync(int recipientId)
        {
            using(var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(
                                    "ForumAppSchema.spFriendRequest_Delete"
                                    , new {RecipientId = recipientId}
                                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteBySenderIdAsync(int senderId)
        {
            using(var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(
                                    "ForumAppSchema.spFriendRequest_Delete"
                                    , new {SenderId = senderId}
                                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<IEnumerable<FriendRequest>> GetAllAsync()
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<FriendRequest> allFriendRequests = await connection.QueryAsync<FriendRequest>(
                    "ForumAppSchema.spFriendRequest_Get"
                    , commandType: CommandType.StoredProcedure
                );
                return allFriendRequests;
            }
        }

        public async Task<FriendRequest?> GetAsync(int senderId, int recipientId)
        {
            using(var connection = _context.CreateConnection())
            {
                FriendRequest? friendRequest = await connection.QuerySingleAsync<FriendRequest>(
                    "ForumAppSchema.spFriendRequest_Get"
                    , new {SenderId = senderId, RecipientId = recipientId}
                    , commandType: CommandType.StoredProcedure
                );
                return friendRequest;
            }
        }

        public async Task<IEnumerable<FriendRequest>> GetByRecipientIdAsync(int recipientId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<FriendRequest> recipientFriendRquests = await connection.QueryAsync<FriendRequest>(
                    "ForumAppSchema.spFriendRequest_Get"
                    , new {RecipientId = recipientId}
                    , commandType: CommandType.StoredProcedure
                );
                return recipientFriendRquests;
            }
        }

        public async Task<IEnumerable<FriendRequest>> GetBySenderIdAsync(int senderId)
        {
            using(var connection = _context.CreateConnection())
            {
                IEnumerable<FriendRequest> senderFriendRquests = await connection.QueryAsync<FriendRequest>(
                    "ForumAppSchema.spFriendRequest_Get"
                    , new {SenderId = senderId}
                    , commandType: CommandType.StoredProcedure
                );
                return senderFriendRquests;
            }
        }
    }
}