using ForumProject.Api.Controllers;
using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;
using Moq;

namespace ForumProject.Tests.UnitTests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }
    private UserDto CreateExampleUserDTO()
    {
        var user = new UserDto
        {
            UserId = 123,
            Username = "john",
            Email = "john@example.com",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return user;
    }

    [Fact]
    public async Task GetUser_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var dto = CreateExampleUserDTO();

        _userServiceMock
            .Setup(x => x.GetByIdAsync(123))
            .ReturnsAsync(dto);

        //Act
        var result = await _controller.GetUser(123);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(dto, result.Value);
    }

    [Fact]
    public async Task GetUser_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int userId = 99;

        _userServiceMock
            .Setup(x => x.GetByIdAsync(userId))
            .ThrowsAsync(new NotFoundException(nameof(User), userId));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _controller.GetUser(userId));
    }
}