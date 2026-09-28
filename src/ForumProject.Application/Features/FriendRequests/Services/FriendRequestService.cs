using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Friendships.DTOs;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.FriendRequests.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipService _friendshipService;

        public  FriendRequestService(IFriendRequestRepository friendRequestRepository
            , IUserRepository userRepository
            , IFriendshipService friendshipService)
        {
            _friendRequestRepository = friendRequestRepository;
            _userRepository = userRepository;
            _friendshipService = friendshipService;
        }

        public async Task<bool> AddAsync(FriendRequestDto friendRequestDto)
        {
            var existingFriendRequest = await _friendRequestRepository.GetAsync(
                friendRequestDto.SenderId, friendRequestDto.RecipientId);
            if (existingFriendRequest != null)
            {
                User recipient = await _userRepository.GetByIdAsync(friendRequestDto.RecipientId)
                    ?? throw new NotFoundException($"recipient {nameof(User)}", 
                        friendRequestDto.RecipientId);
                throw new ConflictException(
                    $"A a friend request to '{recipient.Username}' already exists");
            } 
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

        private async Task<IEnumerable<FriendRequest>> GetByUserId(string option, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException($"{option} {nameof(User)}", userId);

            if(option == "sender")
                return await _friendRequestRepository.GetBySenderIdAsync(userId);
            return await _friendRequestRepository.GetByRecipientIdAsync(userId);
        }

        public async Task<IEnumerable<FriendRequest>> GetByRecipientIdAsync(int recipientId)
        {
            return await GetByUserId("recipient", recipientId);
        }

        public async Task<IEnumerable<FriendRequest>> GetBySenderIdAsync(int senderId)
        {
            return await GetByUserId("sender", senderId);
        }

        public async Task<bool> AcceptFriendRequestAsync(FriendRequestDto friendRequestDto)
        {
            UpdateFriendDto addFriendDto = new UpdateFriendDto
            {
                UserId = friendRequestDto.RecipientId,
                FriendId = friendRequestDto.SenderId
            };

            bool isAnyRowAffected1 = await _friendshipService.CreateFriendship(addFriendDto);
            bool isAnyRowAffected2 = await DeleteAsync(friendRequestDto);

            return isAnyRowAffected1 && isAnyRowAffected2;
        }
    }
}