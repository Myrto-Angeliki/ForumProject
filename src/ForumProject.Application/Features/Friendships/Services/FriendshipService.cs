using AutoMapper;
using ForumProject.Application.Features.Friendships.DTOs;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Friendships.Services;

public class FriendshipService : IFriendshipService
{
    private readonly IFriendshipRepository _friendshipRepository;
    private readonly IMapper _mapper;

    public FriendshipService(IFriendshipRepository friendshipRepository)
    {
        _friendshipRepository = friendshipRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
            }));
    }

    public async Task<IEnumerable<UserDto>> GetFriendsByIdAsync(int userId)
    {
        IEnumerable<User> friends = await _friendshipRepository.GetByUserAsync(userId);
            return friends.Select(_mapper.Map<User, UserDto>);
    }

    private async Task<bool> UpdateFriendship(string action, UpdateFriendDto updateFriendDto)
        {
            if(action == "add")
                return await _friendshipRepository.AddAsync(updateFriendDto.UserId
                    , updateFriendDto.FriendId);
            else
                return await _friendshipRepository.DeleteAsync(updateFriendDto.UserId
                    , updateFriendDto.FriendId);
        }
    public async Task<bool> CreateFriendship(UpdateFriendDto dto)
    {
        return await UpdateFriendship("add", dto);
    }

    public async Task<bool> DeleteFriendship(UpdateFriendDto dto)
    {
        return await UpdateFriendship("remove", dto);
    }
}