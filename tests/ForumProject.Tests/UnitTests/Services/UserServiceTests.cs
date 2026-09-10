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

    private IUserService CreateService(
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

    [Fact]
    public async Task UpdateStatusAsync_WhenUserIsActive_UpdateUserWithNoDeactivatedAt()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.UpdateStatusAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        var service = CreateService(userRepository);

        var dto = new UpdateStatusDto
        {
            UserId = 123,
            IsActive = true
        };

        //Act
        var result = await service.UpdateStatusAsync(dto);

        //Assert
        Assert.True(result);

        userRepository.Verify(
            x => x.UpdateStatusAsync(It.Is<User>(user => 
                user.UserId == 123 &&
                user.IsActive &&
                user.DeactivatedAt == null)),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateStatusAsync_WhenUserIsInactive_UpdateUserWithDeactivatedAt()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.UpdateStatusAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        var service = CreateService(userRepository);

        var dto = new UpdateStatusDto
        {
            UserId = 123,
            IsActive = false
        };

        //Act
        var before = DateTime.UtcNow;

        var result = await service.UpdateStatusAsync(dto);

        var after = DateTime.UtcNow;

        //Assert
        Assert.True(result);

        userRepository.Verify(
            x => x.UpdateStatusAsync(It.Is<User>(user => 
                user.UserId == 123 &&
                !user.IsActive &&
                user.DeactivatedAt >= before &&
                user.DeactivatedAt <= after)),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateEmailAsync_WhenEmailDoesNotExist_ReturnsTrue()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync((User?)null);
        userRepository
            .Setup(x => x.UpdateEmailAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        var service = CreateService(userRepository);

        var dto = new UpdateEmailDto
        {
            UserId = 123,
            Email = "john@example.com"
        };

        //Act
        var result = await service.UpdateEmailAsync(dto);

        //Assert
        Assert.True(result);

        userRepository.Verify(
            x => x.GetByEmailAsync("john@example.com"),
            Times.Once
        );
        userRepository.Verify(
            x => x.UpdateEmailAsync(
                It.Is<User>(user => 
                    user.UserId == 123 &&
                    user.Email == "john@example.com")),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateEmailAsync_WhenEmailAlreadyExists_ThrowConflictException()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        var existingUser = CreateExampleUser(DateTime.UtcNow);
        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync(existingUser);

        var service = CreateService(userRepository);

        var dto = new UpdateEmailDto
        {
            UserId = 123,
            Email = "john@example.com"
        };

        //Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            service.UpdateEmailAsync(dto)
        );

        userRepository.Verify(
            x => x.GetByEmailAsync("john@example.com"),
            Times.Once
        );
        userRepository.Verify(
            x => x.UpdateEmailAsync(It.IsAny<User>()),
            Times.Never
        );
    }

    [Fact]
    public async Task UpdateEmailAsync_WhenRepositoryUpdateFails_ReturnsFalse()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByEmailAsync("john@example.com"))
            .ReturnsAsync((User?)null);
        userRepository
            .Setup(x => x.UpdateEmailAsync(It.IsAny<User>()))
            .ReturnsAsync(false);

        var service = CreateService(userRepository);

        var dto = new UpdateEmailDto
        {
            UserId = 123,
            Email = "john@example.com"
        };

        //Act 
        var result = await service.UpdateEmailAsync(dto);

        //Assert
        Assert.False(result);

        userRepository.Verify(
            x => x.GetByEmailAsync("john@example.com"),
            Times.Once
        );
        userRepository.Verify(
            x => x.UpdateEmailAsync(It.IsAny<User>()),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateUsernameAsync_WhenUsernameDoesNotExist_ReturnsTrue()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByUsernameAsync("john"))
            .ReturnsAsync((User?)null);
        userRepository
            .Setup(x => x.UpdateUsernameAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        var service = CreateService(userRepository);

        var dto = new UpdateUsernameDto
        {
            UserId = 123,
            Username = "john"
        };

        //Act
        var result = await service.UpdateUsernameAsync(dto);

        //Assert
        Assert.True(result);

        userRepository.Verify(
            x => x.GetByUsernameAsync("john"),
            Times.Once
        );
        userRepository.Verify(
            x => x.UpdateUsernameAsync(
                It.Is<User>(user => 
                    user.UserId == 123 &&
                    user.Username == "john")),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateUsernameAsync_WhenUsernameAlreadyExists_ThrowConflictException()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        var existingUser = CreateExampleUser(DateTime.UtcNow);
        userRepository
            .Setup(x => x.GetByUsernameAsync("john"))
            .ReturnsAsync(existingUser);

        var service = CreateService(userRepository);

        var dto = new UpdateUsernameDto
        {
            UserId = 123,
            Username = "john"
        };

        //Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            service.UpdateUsernameAsync(dto)
        );

        userRepository.Verify(
            x => x.GetByUsernameAsync("john"),
            Times.Once
        );
        userRepository.Verify(
            x => x.UpdateUsernameAsync(It.IsAny<User>()),
            Times.Never
        );
    }

    [Fact]
    public async Task UpdateUsernameAsync_WhenRepositoryUpdateFails_ReturnsFalse()
    {
        //Arrange
        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(x => x.GetByUsernameAsync("john"))
            .ReturnsAsync((User?)null);
        userRepository
            .Setup(x => x.UpdateUsernameAsync(It.IsAny<User>()))
            .ReturnsAsync(false);

        var service = CreateService(userRepository);

        var dto = new UpdateUsernameDto
        {
            UserId = 123,
            Username = "john"
        };

        //Act 
        var result = await service.UpdateUsernameAsync(dto);

        //Assert
        Assert.False(result);

        userRepository.Verify(
            x => x.GetByUsernameAsync("john"),
            Times.Once
        );
        userRepository.Verify(
            x => x.UpdateUsernameAsync(It.IsAny<User>()),
            Times.Once
        );
    }
}
