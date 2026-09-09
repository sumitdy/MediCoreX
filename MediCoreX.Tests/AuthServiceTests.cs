using MediCoreX.Api.Data;
using MediCoreX.Api.DTOs;
using MediCoreX.Api.Models;
using MediCoreX.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace MediCoreX.Tests;

public class AuthServiceTests
{
    private readonly MediCoreXDbContext _context;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<ILogger<AuthService>> _mockLogger;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<MediCoreXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new MediCoreXDbContext(options);

        _mockTokenService = new Mock<ITokenService>();

        _mockLogger = new Mock<ILogger<AuthService>>();
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsTokens()
    {
        // Arrange

        var user = new User
        {
            Id = 1,
            FullName = "Test User",
            Email = "test@gmail.com",
            Role = "User"
        };

        var passwordHasher = new PasswordHasher<User>();

        user.PasswordHash = passwordHasher.HashPassword(
            user,
            "Password123"
        );

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        _mockTokenService
            .Setup(x => x.CreateToken(It.IsAny<User>()))
            .Returns("fake-access-token");

        _mockTokenService
            .Setup(x => x.GenerateRefreshToken())
            .Returns("fake-refresh-token");

        var service = new AuthService(
            _context,
            _mockTokenService.Object,
            _mockLogger.Object
        );

        var loginDto = new LoginDto
        {
            Email = "test@gmail.com",
            Password = "Password123"
        };

        // Act

        var result = await service.LoginAsync(loginDto);

        // Assert

        Assert.NotNull(result);

        Assert.Equal(
            "fake-access-token",
            result.AccessToken
        );

        Assert.Equal(
            "fake-refresh-token",
            result.RefreshToken
        );
    }
}