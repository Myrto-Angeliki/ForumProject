using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Application.Features.Users.Services;
using ForumProject.Domain.Entities;
using Moq;

namespace ForumProject.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task DeleteUserAsync_WhenRepositoryReturnsTrue_ReturnTrue()
    {
        // Arrange

        var userRepository = new Mock<IUserRepository>();
        var friendshipRepository = new Mock<IFriendshipRepository>();
        var friendRequestService = new Mock<IFriendRequestService>();

        userRepository
            .Setup(x => x.DeleteAsync(123))
            .ReturnsAsync(true);

        var service = new UserService(
            userRepository.Object,
            friendshipRepository.Object,
            friendRequestService.Object
        );

        //Act
        var result = await service.DeleteUserAsync(123);

        //Assert
        Assert.True(result);

        userRepository.Verify(
            x => x.DeleteAsync(123),
            Times.Once
        );
    }
}
