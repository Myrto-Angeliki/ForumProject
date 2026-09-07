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

        public async Task<bool> AddAsync(FriendRequestDto friendRequestDto)
        {
            bool isAnyRowAffected = await _friendRequestRepository.AddAsync(
                friendRequestDto.SenderId
                , friendRequestDto.RecipientId);

            return isAnyRowAffected;
        }

        public async Task<bool> DeleteAsync(FriendRequestDto friendRequestDto)
        {
            bool isAnyRowAffected = await _friendRequestRepository.DeleteAsync(
                friendRequestDto.SenderId
                , friendRequestDto.RecipientId);

            return isAnyRowAffected;
        }

        public async Task<bool> DeleteByRecipientIdAsync(int recipientId)
        {
            bool isAnyRowAffected = await _friendRequestRepository
                .DeleteByRecipientIdAsync(recipientId);
            
            return isAnyRowAffected;
        }

        public async Task<bool> DeleteBySenderIdAsync(int senderId)
        {
            bool isAnyRowAffected = await _friendRequestRepository
                .DeleteBySenderIdAsync(senderId);
            
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