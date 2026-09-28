using System.Security.Claims;
using ForumProject.Api.Controllers;
using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.Auths.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ForumProject.Tests.UnitTests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    private void SetUserContext(string? claimType, string? claimValue)
    {
        var claims = new List<Claim>();
        if (claimType != null && claimValue != null)
        {
            claims.Add(new Claim(claimType, claimValue));
        }

        var identity = new ClaimsIdentity(claims, "TestAuth");
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    [Fact]
    public async Task RefreshToken_WithUserIdClaimWhenUserExists_ParsesIdAndReturnsOk()
    {
        // Arrange
        int userId = 99;
        string expectedToken = "new_jwt_token";
        SetUserContext("userId", userId.ToString());

        _authServiceMock
            .Setup(x => x.RefreshTokenAsync(userId))
            .ReturnsAsync(expectedToken);

        //Act
        var result = await _controller.RefreshToken();

        //Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedToken, okResult.Value);
    }

    [Fact]
    public async Task RefreshToken_WithFallbackNameIdentifierClaim_ParsesIdAndReturnsOk()
    {
        // Arrange
        int userId = 99;
        string expectedToken = "fallback_jwt_token";
        SetUserContext(ClaimTypes.NameIdentifier, userId.ToString());

        _authServiceMock
            .Setup(s => s.RefreshTokenAsync(userId))
            .ReturnsAsync(expectedToken);

        // Act
        IActionResult result = await _controller.RefreshToken();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedToken, okResult.Value);
    }

    [Fact]
    public async Task RefreshToken_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int userId = 99;
        SetUserContext("userId", userId.ToString());

        _authServiceMock
            .Setup(x => x.RefreshTokenAsync(userId))
            .ThrowsAsync(new NotFoundException(nameof(User), userId));

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _controller.RefreshToken());
    }
    
}