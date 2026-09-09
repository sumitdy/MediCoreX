using MediCoreX.Api.Data;
using MediCoreX.Api.DTOs;
using MediCoreX.Api.Exceptions;
using MediCoreX.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediCoreX.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly MediCoreXDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _hasher = new();
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            MediCoreXDbContext context,
            ITokenService tokenService,
            ILogger<AuthService> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new BadRequestException("User already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = "User"
            };

            user.PasswordHash = _hasher.HashPassword(
                user,
                dto.Password
            );

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new UnauthorizedException("Invalid credentials");

            var result = _hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Invalid credentials");

            // Generate Access Token
            var accessToken = _tokenService.CreateToken(user);

            // Generate Refresh Token
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Save Refresh Token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // Find user using Refresh Token
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null)
                throw new UnauthorizedException("Invalid refresh token");

            // Check Refresh Token expiry
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new UnauthorizedException("Refresh token expired");

            // Generate new Access Token
            var newAccessToken = _tokenService.CreateToken(user);

            // Generate new Refresh Token
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Replace old Refresh Token
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}