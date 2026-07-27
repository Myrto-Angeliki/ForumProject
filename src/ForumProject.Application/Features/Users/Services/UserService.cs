using AutoMapper;
using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ForumProject.Application.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IMapper _mapper;

        public  UserService(IUserRepository userRepository
            , IFriendshipRepository friendshipRepository
            , IFriendRequestService friendRequestService)
        {
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
            _friendRequestService = friendRequestService;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateUserDto, User>();
                cfg.CreateMap<UpdateFriendDto, FriendRequestDto>();
            }));
        }

        private async Task<bool> UpdateFriendship(UpdateFriendDto updateFriendDto)
        {
            User? user = await _userRepository.GetByIdAsync(updateFriendDto.UserId);
            User? friend = await _userRepository.GetByIdAsync(updateFriendDto.FriendId);
            if(friend != null && user != null)
            {
                user.UpdateFriends(friend, updateFriendDto.Action);
                if(updateFriendDto.Action == "add")
                {
                    return await _friendshipRepository.AddAsync(updateFriendDto.UserId
                        , updateFriendDto.FriendId);
                }
                else
                {
                    return await _friendshipRepository.DeleteAsync(updateFriendDto.UserId
                        , updateFriendDto.FriendId);
                }
            }
            throw new Exception("Failed to remove friend!");
        }

        public async Task<bool> AddFriend(UpdateFriendDto addFriendDto)
        {
            addFriendDto.Action = "add";
            bool isAnyRowAffected1 = await UpdateFriendship(addFriendDto);

            FriendRequestDto friendRequestDto = _mapper.Map<FriendRequestDto>(addFriendDto);
            friendRequestDto.Action = "remove";
            bool isAnyRowAffected2 = await _friendRequestService.DeleteAsync(friendRequestDto);

            return isAnyRowAffected1 && isAnyRowAffected2;
        }

        public async Task<bool> DeleteFriendAsync(UpdateFriendDto removeFriendDto)
        {
            removeFriendDto.Action = "remove";
            return await UpdateFriendship(removeFriendDto);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            return await _userRepository.DeleteAsync(userId); 
        }

        public async Task<IEnumerable<User>> GetFriendsByIdAsync(int userId)
        {
            return await _friendshipRepository.GetByUserAsync(userId);
        }

        public async Task<bool> UpdateUser(UpdateUserDto userDto)
        {
            User userToUpdate = _mapper.Map<User>(userDto);
            return await _userRepository.UpdateAsync(userToUpdate);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            
            if(user != null)
            {
                return user;
            }
            throw new Exception("User not found!");
        }
    }
}