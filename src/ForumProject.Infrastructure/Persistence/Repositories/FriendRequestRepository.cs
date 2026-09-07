using System.Data;
using Dapper;
using ForumProject.Domain.Entities;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Infrastructure.Persistence;
using ForumProject.Application.Features.Users.DTOs;
using AutoMapper;
using ForumProject.Application.Features.FriendRequests.DTOs;

namespace ForumProject.Infrastructure.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly DataContextDapper _context;
        private readonly IMapper _mapper;

        public FriendRequestRepository(DataContextDapper context)
        {
            _context = context;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, User>();
            }));
        }

        public async Task<bool> AddAsync(int senderId, int recipientId)
        {
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(
                                    "ForumAppSchema.spFriendRequest_Insert"
                                    , new { SenderId = senderId, RecipientId = recipientId }
                                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        private async Task<bool> ExecuteDelete(int? senderId = null, int? recipientId = null)
        {
            using (var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(
                                    "ForumAppSchema.spFriendRequest_Delete"
                                    , new { SenderId = senderId, RecipientId = recipientId }
                                    , commandType: CommandType.StoredProcedure
                );
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(int senderId, int recipientId)
        {
            return await ExecuteDelete(senderId, recipientId);
        }

        public async Task<bool> DeleteByRecipientIdAsync(int recipientId)
        {
            return await ExecuteDelete(recipientId: recipientId);
        }

        public async Task<bool> DeleteBySenderIdAsync(int senderId)
        {
            return await ExecuteDelete(senderId: senderId);
        }

        private async Task<IEnumerable<FriendRequest>> ExecuteGetQuery(
            int? senderId = null,
            int? recipientId = null)
        {
            using var connection = _context.CreateConnection();

            var requests = await connection.QueryAsync<FriendRequestQueryDto>(
                "ForumAppSchema.spFriendRequest_Get",
                new
                {
                    SenderId = senderId,
                    RecipientId = recipientId
                },
                commandType: CommandType.StoredProcedure
            );

            Console.WriteLine($"Argument Sender: {senderId}");
            Console.WriteLine($"Argument Recipient: {(recipientId != null ? recipientId : "null")}");
            foreach (var row in requests)
            {
                Console.WriteLine($"Sender: {row.SenderId}");
                Console.WriteLine($"Recipient: {row.RecipientId}");
            }

            return requests.Select(x =>
                new FriendRequest(
                    new User
                    {
                        UserId = x.SenderId,
                        Username = x.SenderUsername,
                        Email = x.SenderEmail,
                        IsActive = x.SenderIsActive,
                        CreatedAt = x.SenderCreatedAt,
                        UpdatedAt = x.SenderUpdatedAt,
                        DeactivatedAt = x.SenderDeactivatedAt
                    },
                    new User
                    {
                        UserId = x.RecipientId,
                        Username = x.RecipientUsername,
                        Email = x.RecipientEmail,
                        IsActive = x.RecipientIsActive,
                        CreatedAt = x.RecipientCreatedAt,
                        UpdatedAt = x.RecipientUpdatedAt,
                        DeactivatedAt = x.RecipientDeactivatedAt
                    },
                    x.FriendRequestUpdatedAt
                )
            );
        }

        public async Task<IEnumerable<FriendRequest>> GetAllAsync()
        {
            IEnumerable<FriendRequest> allFriendRequests = await ExecuteGetQuery();
            return allFriendRequests;
        }

        public async Task<FriendRequest?> GetAsync(int senderId, int recipientId)
        {
            IEnumerable<FriendRequest> friendRequests = await ExecuteGetQuery(
                senderId
                , recipientId
            );

            if (friendRequests.Count() <= 1)
                return friendRequests.ToList().FirstOrDefault();
            throw new Exception(@"Unexpected behaviour from FriendRequest entity. " +
                "There cannot be many friend requests for the same user and sender");
        }

        public async Task<IEnumerable<FriendRequest>> GetByRecipientIdAsync(int recipientId)
        {
            IEnumerable<FriendRequest> recipientFriendRquests = await ExecuteGetQuery(
                recipientId: recipientId
            );
            return recipientFriendRquests;
        }

        public async Task<IEnumerable<FriendRequest>> GetBySenderIdAsync(int senderId)
        {
            IEnumerable<FriendRequest> senderFriendRquests = await ExecuteGetQuery(
                senderId: senderId
            );
            return senderFriendRquests;
        }
    }
}