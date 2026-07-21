using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRepository _friendshipRepository;

        public  UserService(IUserRepository userRepository, IFriendshipRepository friendshipRepository)
        {
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
        }

        private async Task<bool> UpdateFriendship(UpdateFriendDto updateFriendDto, string action)
        {
            User? user = await _userRepository.GetByIdAsync(updateFriendDto.UserId);
            User? friend = await _userRepository.GetByIdAsync(updateFriendDto.FriendId);
            if(friend != null && user != null)
            {
                user.UpdateFriends(friend, action);
                if(action == "add")
                return await _friendshipRepository.AddAsync(updateFriendDto.UserId
                        , updateFriendDto.FriendId);
                else
                    return await _friendshipRepository.DeleteAsync(updateFriendDto.UserId
                        , updateFriendDto.FriendId);
            }
            throw new Exception("Failed to remove friend!");
        }

        public async Task<bool> AddFriend(UpdateFriendDto addFriendDto)
        {
            return await UpdateFriendship(addFriendDto, "add");
        }

        public async Task<bool> DeleteFriendAsync(UpdateFriendDto removeFriendDto)
        {
            return await UpdateFriendship(removeFriendDto, "remove");
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
            //@TODO
            //create mapper
            //handle deactivate action
            User userToUpdate = new User
            {
                UserId = userDto.UserId,
                Email = userDto.Email,
                Username = userDto.Username,
                IsActive = userDto.IsActive
            };
            return await _userRepository.UpdateAsync(userToUpdate);
        }
    }
}