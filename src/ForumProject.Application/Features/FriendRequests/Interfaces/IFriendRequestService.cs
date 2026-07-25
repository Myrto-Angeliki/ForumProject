using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.FriendRequests.Interfaces
{
    public interface IFriendRequestService
    {
        Task<FriendRequest?> GetAsync(int senderId, int recipientId);
        Task<IEnumerable<FriendRequest>> GetBySenderAsync(int senderId);
        Task<IEnumerable<FriendRequest>> GetByRecipientAsync(int recipientId);
        Task<bool> AddAsync(int senderId, int recipientId);
        Task<bool> DeleteAsync(int senderId, int recipientId);
        Task<bool> DeleteBySenderAsync(int senderId);
        Task<bool> DeleteByRecipientAsync(int recipientId);

    }
}