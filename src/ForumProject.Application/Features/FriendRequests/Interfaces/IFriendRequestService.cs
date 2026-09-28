using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.FriendRequests.Interfaces
{
    public interface IFriendRequestService
    {
        Task<FriendRequest?> GetAsync(FriendRequestDto friendRequestDto);
        Task<IEnumerable<FriendRequest>> GetBySenderIdAsync(int senderId);
        Task<IEnumerable<FriendRequest>> GetByRecipientIdAsync(int recipientId);
        Task<bool> AddAsync(FriendRequestDto friendRequestDto);
        Task<bool> DeleteAsync(FriendRequestDto friendRequestDto);
        Task<bool> DeleteBySenderIdAsync(int senderId);
        Task<bool> DeleteByRecipientIdAsync(int recipientId);
        Task<bool> DeleteByUserId(int userId);
        Task<bool> AcceptFriendRequestAsync(FriendRequestDto friendRequestDto);

    }
}