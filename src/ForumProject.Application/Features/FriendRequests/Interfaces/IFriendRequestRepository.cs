using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.FriendRequests.Interfaces
{
    public interface IFriendRequestRepository
    {
        Task<IEnumerable<FriendRequest>> GetAllAsync();
        Task<FriendRequest?> GetAsync(int senderId, int recipientId);
        Task<IEnumerable<FriendRequest>> GetBySenderIdAsync(int senderId);
        Task<IEnumerable<FriendRequest>> GetByRecipientIdAsync(int recipientId);
        Task<bool> AddAsync(int senderId, int recipientId);
        Task<bool> DeleteAsync(int senderId, int recipientId);
        Task<bool> DeleteBySenderIdAsync(int senderId);
        Task<bool> DeleteByRecipientIdAsync(int recipientId);
    }
}