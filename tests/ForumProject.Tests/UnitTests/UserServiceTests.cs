using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.FriendRequests.Interfaces;
using ForumProject.Application.Features.Friendships.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Application.Features.Users.Services;
using ForumProject.Domain.Entities;
using Moq;

namespace ForumProject.Tests.UnitTests;

public class UserServiceTests
{

    private UserService CreateService(
        Mock<IUserRepository> userRepository)
    {
        var friendshipRepository = new Mock<IFriendshipRepository>();
        var friendRequestService = new Mock<IFriendRequestService>();

        return new UserService(
            userRepository.Object,
            friendshipRepository.Object,
            friendRequestService.Object);
    }

    private User CreateExampleUser(DateTime creationDateTime)
    {
        var user = new User
        {
            UserId = 123,
            Username = "john",
            Email = "john@example.com",
            IsActive = true,
            CreatedAt = creationDateTime,
            UpdatedAt = creationDateTime
        };
        return user;
    }

    private void AssertExampleUserDetails(UserDto? result, DateTime creationDateTime)
    {
        Assert.NotNull(result);
        Assert.IsType<UserDto>(result);
        Assert.Equal(123, result.UserId);
        Assert.Equal("john", result.Username);
        Assert.Equal("john@example.com", result.Email);
        Assert.True(result.IsActive);
        Assert.Equal(creationDateTime, result.CreatedAt);
        Assert.Equal(creationDateTime, result.UpdatedAt);
    }


    [Fact]
    public async Task DeleteUserAsync_WhenRepositoryReturnsTrue_ReturnTrue()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.DeleteAsync(123))
            .ReturnsAsync(true);

        var service = CreateService(userRepository);

        //Act
        var result = await service.DeleteUserAsync(123);

        //Assert
        Assert.True(result);

        userRepository.Verify(
            x => x.DeleteAsync(123),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteUserAsync_WhenRepositoryReturnsFalse_ReturnFalse()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.DeleteAsync(123))
            .ReturnsAsync(false);

        var service = CreateService(userRepository);

        //Act
        var result = await service.DeleteUserAsync(123);

        //Assert
        Assert.False(result);

        userRepository.Verify(
            x => x.DeleteAsync(123),
            Times.Once
        );
    }


    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var creationDateTime = DateTime.UtcNow;
        var user = CreateExampleUser(creationDateTime);

        userRepository
            .Setup(x => x.GetByIdAsync(123))
            .ReturnsAsync(user);

        var service = CreateService(userRepository);

        // Act
        var result = await service.GetByIdAsync(123);

        // Assert
        AssertExampleUserDetails((UserDto?)result, creationDateTime);
    }


    [Fact]
    public async Task GetByIdAsync_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByIdAsync(123))
            .ReturnsAsync((User?)null);

        var service = CreateService(userRepository);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(123));
    }

    [Fact]
    public async Task GetByEmailAsync_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var creationDateTime = DateTime.UtcNow;
        var user = CreateExampleUser(creationDateTime);

        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync(user);

        var service = CreateService(userRepository);

        // Act
        var result = await service.GetByEmailAsync("john@example.com");

        // Assert
        AssertExampleUserDetails((UserDto?)result, creationDateTime);
    }


    [Fact]
    public async Task GetByEmailAsync_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync((User?)null);

        var service = CreateService(userRepository);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByEmailAsync("john@example.com"));
    }

    [Fact]
    public async Task GetByUsernameAsync_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var creationDateTime = DateTime.UtcNow;
        var user = CreateExampleUser(creationDateTime);

        userRepository
            .Setup(x => x.GetByUsernameAsync("john"))
            .ReturnsAsync(user);

        var service = CreateService(userRepository);

        // Act
        var result = await service.GetByUsernameAsync("john");

        // Assert
        AssertExampleUserDetails((UserDto?)result, creationDateTime);
    }


    [Fact]
    public async Task GetByUsernameAsync_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByUsernameAsync("john"))
            .ReturnsAsync((User?)null);

        var service = CreateService(userRepository);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByUsernameAsync("john"));
    }
}
