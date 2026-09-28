using System.ComponentModel;
using System.Drawing;
using AutoMapper;
using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.FriendRequests.DTOs;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Friendships.DTOs;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipService _friendshipService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IMapper _mapper;

        public  UserService(IUserRepository userRepository
            , IFriendshipService friendshipService
            , IFriendRequestService friendRequestService)
        {
            _userRepository = userRepository;
            _friendshipService = friendshipService;
            _friendRequestService = friendRequestService;
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateStatusDto, User>();
                cfg.CreateMap<UpdateEmailDto, User>();
                cfg.CreateMap<UpdateUsernameDto, User>();
                cfg.CreateMap<UpdateFriendDto, FriendRequestDto>();
                cfg.CreateMap<User, UserDto>();
            }));
        }

        public async Task<bool> DeleteFriendAsync(UpdateFriendDto removeFriendDto)
        {
            return await _friendshipService.DeleteFriendship(removeFriendDto);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteAsync(userId); 
        }

        public async Task<IEnumerable<UserDto>> GetFriendsByIdAsync(int userId)
        {
            return await _friendshipService.GetFriendsByIdAsync(userId);
        }

        public async Task<bool> UpdateStatusAsync(UpdateStatusDto dto)
        {
            User user = _mapper.Map<User>(dto);
            user.DeactivatedAt = user.IsActive ? null : DateTime.UtcNow;
            //Console.WriteLine($"userId: {user.UserId}, IsActive: {user.IsActive}, DeactivatedAt: {user.DeactivatedAt}");
            return await _userRepository.UpdateStatusAsync(user);
        }

        public async Task<bool> UpdateEmailAsync(UpdateEmailDto dto)
        {
            var userWithThatEmailAlreadyExists = await _userRepository
                                                    .GetByEmailAsync(dto.Email);
            if(userWithThatEmailAlreadyExists != null)
                throw new ConflictException(nameof(User), nameof(dto.Email), dto.Email);
            return await _userRepository.UpdateEmailAsync(_mapper.Map<User>(dto));
        }

        public async Task<bool> UpdateUsernameAsync(UpdateUsernameDto dto)
        {
            var userWithThatUsernameAlreadyExists = await _userRepository
                                                    .GetByUsernameAsync(dto.Username);
            if(userWithThatUsernameAlreadyExists != null)
                throw new ConflictException(nameof(User), nameof(dto.Username), dto.Username);
            return await _userRepository.UpdateUsernameAsync(_mapper.Map<User>(dto));
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
                throw new InvalidOptionException("invalid option: get user by "+option);
            
            if(user != null)
                return _mapper.Map<UserDto>(user);
            throw new NotFoundException(nameof(User), optionParam);
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