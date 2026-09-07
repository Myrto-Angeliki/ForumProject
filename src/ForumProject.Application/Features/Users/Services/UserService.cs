using System.Drawing;
using AutoMapper;
using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
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
                cfg.CreateMap<User, UpdateUserDto>();
                cfg.CreateMap<User, UserDto>();
            }));
        }

        private async Task<bool> UpdateFriendship(UpdateFriendDto updateFriendDto)
        {
            if(updateFriendDto.Action == "add")
                return await _friendshipRepository.AddAsync(updateFriendDto.UserId
                    , updateFriendDto.FriendId);
            else
                return await _friendshipRepository.DeleteAsync(updateFriendDto.UserId
                    , updateFriendDto.FriendId);
        }

        public async Task<bool> AddFriendAsync(UpdateFriendDto addFriendDto)
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

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteAsync(userId); 
        }

        public async Task<IEnumerable<UserDto>> GetFriendsByIdAsync(int userId)
        {
            IEnumerable<User> friends = await _friendshipRepository.GetByUserAsync(userId);
            return friends.Select(_mapper.Map<User, UserDto>);
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDto userDto)
        {
            return await _userRepository.UpdateAsync(_mapper.Map<User>(userDto));
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            IEnumerable<User> users = await _userRepository.GetAllAsync();
            return users.Select(_mapper.Map<User, UserDto>);
        }

        private async Task<UserDto> GetUserByOption(
            string option, string optionParam)
        {
            User? user;
            if(option == "id")
                user = await _userRepository.GetByIdAsync(Int32.Parse(optionParam));
            else if(option == "email")
                user = await _userRepository.GetByEmailAsync(optionParam);
            else if(option == "username")
                user = await _userRepository.GetByUsernameAsync(optionParam);
            else
                throw new Exception("invalid option: "+option);
            
            if(user == null)
                throw new NotFoundException(nameof(User), optionParam);
                
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByIdAsync(int userId)
        {
            return await GetUserByOption(option: "id", userId.ToString());
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            return await GetUserByOption(option: "email", email);
        }

        public async Task<UserDto> GetByUsernameAsync(string username)
        {
            return await GetUserByOption(option: "username", username);
        }
    }
}