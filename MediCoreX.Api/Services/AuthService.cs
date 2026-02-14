using MediCoreX.Api.Data;
using MediCoreX.Api.DTOs;
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
               throw new Exception("User already exists");

            var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role
        };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
               await _context.SaveChangesAsync();
        }


        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Invalid credentials");

            var result = _hasher.VerifyHashedPassword(
                user, user.PasswordHash, dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials");

            return _tokenService.CreateToken(user);
        }
    }
}
