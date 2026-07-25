using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Comments.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserRepository _userRepository;

        public  FriendRequestService(IFriendRequestRepository friendRequestRepository
            , IUserRepository userRepository)
        {
            _friendRequestRepository = friendRequestRepository;
            _userRepository = userRepository;
        }

        private async Task UpdateFriendRequests(int senderId, int recipientId
            , string updateAction, bool isCalledBySender)//createDTO?
        {
            User? sender = await _userRepository.GetByIdAsync(senderId);
            User? recipient = await _userRepository.GetByIdAsync(recipientId);
            if(sender != null && recipient != null)
            {
                
            }
        }

        public async Task<bool> AddAsync(int senderId, int recipientId)
        {
            bool isRowAffected = await _friendRequestRepository.AddAsync(senderId, recipientId);

            return isRowAffected;
        }

        public async Task<bool> DeleteAsync(int senderId, int recipientId)
        {
            bool isRowAffected = await _friendRequestRepository.DeleteAsync(senderId, recipientId);
            return isRowAffected;
        }

        public async Task<bool> DeleteByRecipientAsync(int recipientId)
        {
            bool isRowAffected = await _friendRequestRepository.DeleteByRecipientAsync(recipientId);
            return isRowAffected;
        }

        public Task<bool> DeleteBySenderAsync(int senderId)
        {
            throw new NotImplementedException();
        }

        public async Task<FriendRequest?> GetAsync(int senderId, int recipientId)
        {
            return await _friendRequestRepository.GetAsync(senderId, recipientId);
        }

        public async Task<IEnumerable<FriendRequest>> GetByRecipientAsync(int recipientId)
        {
            return await _friendRequestRepository.GetByRecipientAsync(recipientId);
        }

        public async Task<IEnumerable<FriendRequest>> GetBySenderAsync(int senderId)
        {
            return await _friendRequestRepository.GetBySenderAsync(senderId);
        }
    }
}