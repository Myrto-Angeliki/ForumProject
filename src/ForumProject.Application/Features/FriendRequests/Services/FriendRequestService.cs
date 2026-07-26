using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.FriendRequests.Services
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

        private async Task UpdateFriendRequests(FriendRequestDto friendRequestDto)
        {
            FriendRequest? friendRequest = await _friendRequestRepository.GetAsync(
                    friendRequestDto.SenderId
                    , friendRequestDto.RecipientId);
            if(friendRequest != null)
            {
                friendRequest.UpdateUserFriendRequestLists(friendRequestDto.Action);
                return;
            }
            throw new Exception("Cannot update friend request that does not exist!");
        }

        public async Task<bool> AddAsync(FriendRequestDto friendRequestDto)
        {
            bool isAnyRowAffected = await _friendRequestRepository.AddAsync(
                friendRequestDto.SenderId
                , friendRequestDto.RecipientId);

            await UpdateFriendRequests(friendRequestDto);

            return isAnyRowAffected;
        }

        public async Task<bool> DeleteAsync(FriendRequestDto friendRequestDto)
        {
            bool isAnyRowAffected = await _friendRequestRepository.DeleteAsync(
                friendRequestDto.SenderId
                , friendRequestDto.RecipientId);

            await UpdateFriendRequests(friendRequestDto);

            return isAnyRowAffected;
        }

        private async void DeleteByRecipientOrSenderIdAsync(int userId, bool isSender)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            if(user != null)
            {
                if(isSender)
                    user.FriendRequestsSent = new();
                else 
                    user.FriendRequestsReceived = new();
            }
            else
                throw new Exception("User not found!");
        }

        public async Task<bool> DeleteByRecipientIdAsync(int recipientId)
        {
            bool isAnyRowAffected = await _friendRequestRepository.DeleteByRecipientIdAsync(recipientId);
            DeleteByRecipientOrSenderIdAsync(recipientId, isSender: false);
            
            return isAnyRowAffected;
        }

        public async Task<bool> DeleteBySenderIdAsync(int senderId)
        {
            bool isAnyRowAffected = await _friendRequestRepository.DeleteBySenderIdAsync(senderId);
            DeleteByRecipientOrSenderIdAsync(senderId, isSender: true);
            
            return isAnyRowAffected;
        }

        public async Task<bool> DeleteByUserId(int userId)
        {
            bool isAnyRowAffected1 = await DeleteBySenderIdAsync(userId);
            bool isAnyRowAffected2 = await DeleteByRecipientIdAsync(userId);
            
            return isAnyRowAffected1 || isAnyRowAffected2;
        }

        public async Task<FriendRequest?> GetAsync(FriendRequestDto friendRequestDto)
        {
            return await _friendRequestRepository.GetAsync(
                friendRequestDto.SenderId
                , friendRequestDto.RecipientId);
        }

        public async Task<IEnumerable<FriendRequest>> GetByRecipientIdAsync(int recipientId)
        {
            return await _friendRequestRepository.GetByRecipientIdAsync(recipientId);
        }

        public async Task<IEnumerable<FriendRequest>> GetBySenderIdAsync(int senderId)
        {
            return await _friendRequestRepository.GetBySenderIdAsync(senderId);
        }
    }
}