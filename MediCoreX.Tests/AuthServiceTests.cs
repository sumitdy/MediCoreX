using MediCoreX.Api.Data;
using MediCoreX.Api.DTOs;
using MediCoreX.Api.Exceptions;
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

    [Fact]
    public async Task LoginAsync_InvalidEmail_ThrowsUnauthorizedException()
    {
        // Arrange: no user with this email exists in the in-memory database.
        var service = new AuthService(
            _context,
            _mockTokenService.Object,
            _mockLogger.Object
        );

        var loginDto = new LoginDto
        {
            Email = "unknown@example.com",
            Password = "Password123"
        };

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedException>(
            () => service.LoginAsync(loginDto)
        );

        // Assert: do not reveal whether the email exists.
        Assert.Equal("Invalid credentials", exception.Message);
    }

    [Fact]
public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedException()
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

    var service = new AuthService(
        _context,
        _mockTokenService.Object,
        _mockLogger.Object
    );

    var loginDto = new LoginDto
    {
        Email = "test@gmail.com",
        Password = "WrongPassword"
    };

    // Act

    var exception = await Assert.ThrowsAsync<UnauthorizedException>(
        () => service.LoginAsync(loginDto)
    );

    // Assert

    Assert.Equal("Invalid credentials", exception.Message);
}

[Fact]
public async Task RegisterAsync_ValidUser_CreatesUser()
{
    // Arrange

    var service = new AuthService(
        _context,
        _mockTokenService.Object,
        _mockLogger.Object
    );

    var registerDto = new RegisterDto
    {
        FullName = "New User",
        Email = "newuser@gmail.com",
        Password = "Password123"
    };

    // Act

    await service.RegisterAsync(registerDto);

    // Assert

    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == "newuser@gmail.com");

    Assert.NotNull(user);
    Assert.Equal("New User", user.FullName);
    Assert.Equal("User", user.Role);
}

[Fact]
public async Task RegisterAsync_DuplicateEmail_ThrowsBadRequestException()
{
    // Arrange

    var existingUser = new User
    {
        FullName = "Existing User",
        Email = "test@gmail.com",
        Role = "User"
    };

    var passwordHasher = new PasswordHasher<User>();

    existingUser.PasswordHash = passwordHasher.HashPassword(
        existingUser,
        "Password123"
    );

    _context.Users.Add(existingUser);

    await _context.SaveChangesAsync();

    var service = new AuthService(
        _context,
        _mockTokenService.Object,
        _mockLogger.Object
    );

    var registerDto = new RegisterDto
    {
        FullName = "Another User",
        Email = "test@gmail.com",
        Password = "Password456"
    };

    // Act

    var exception = await Assert.ThrowsAsync<BadRequestException>(
        () => service.RegisterAsync(registerDto)
    );

    // Assert

    Assert.Equal("User already exists", exception.Message);
}

[Fact]
public async Task RefreshTokenAsync_ValidToken_ReturnsNewTokens()
{
    // Arrange

    var user = new User
    {
        Id = 1,
        FullName = "Test User",
        Email = "test@gmail.com",
        Role = "User",
        RefreshToken = "old-refresh-token",
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    _mockTokenService
        .Setup(x => x.CreateToken(It.IsAny<User>()))
        .Returns("new-access-token");

    _mockTokenService
        .Setup(x => x.GenerateRefreshToken())
        .Returns("new-refresh-token");

    var service = new AuthService(
        _context,
        _mockTokenService.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.RefreshTokenAsync(
        "old-refresh-token"
    );

    // Assert

    Assert.NotNull(result);

    Assert.Equal(
        "new-access-token",
        result.AccessToken
    );

    Assert.Equal(
        "new-refresh-token",
        result.RefreshToken
    );

    // Verify refresh token rotation

    var updatedUser = await _context.Users
        .FirstAsync(u => u.Id == 1);

    Assert.Equal(
        "new-refresh-token",
        updatedUser.RefreshToken
    );
}

[Fact]
public async Task RefreshTokenAsync_InvalidToken_ThrowsUnauthorizedException()
{
    // Arrange

    var user = new User
    {
        Id = 1,
        FullName = "Test User",
        Email = "test@gmail.com",
        Role = "User",
        RefreshToken = "valid-refresh-token",
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var service = new AuthService(
        _context,
        _mockTokenService.Object,
        _mockLogger.Object
    );

    // Act

    var exception = await Assert.ThrowsAsync<UnauthorizedException>(
        () => service.RefreshTokenAsync("wrong-refresh-token")
    );

    // Assert

    Assert.Equal(
        "Invalid refresh token",
        exception.Message
    );
}

[Fact]
public async Task RefreshTokenAsync_ExpiredToken_ThrowsUnauthorizedException()
{
    // Arrange

    var user = new User
    {
        Id = 1,
        FullName = "Test User",
        Email = "test@gmail.com",
        Role = "User",
        RefreshToken = "expired-refresh-token",
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(-1)
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var service = new AuthService(
        _context,
        _mockTokenService.Object,
        _mockLogger.Object
    );

    // Act

    var exception = await Assert.ThrowsAsync<UnauthorizedException>(
        () => service.RefreshTokenAsync("expired-refresh-token")
    );

    // Assert

    Assert.Equal(
        "Refresh token expired",
        exception.Message
    );
}

}
