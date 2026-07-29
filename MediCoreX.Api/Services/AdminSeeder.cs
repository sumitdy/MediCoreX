using MediCoreX.Api.Configurations;
using MediCoreX.Api.Data;
using MediCoreX.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MediCoreX.Api.Services;

public class AdminSeeder
{
    private readonly MediCoreXDbContext _context;
    private readonly AdminSettings _adminSettings;
    private readonly ILogger<AdminSeeder> _logger;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AdminSeeder(
        MediCoreXDbContext context,
        IOptions<AdminSettings> adminOptions,
        ILogger<AdminSeeder> logger)
    {
        _context = context;
        _adminSettings = adminOptions.Value;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_adminSettings.FullName) ||
            string.IsNullOrWhiteSpace(_adminSettings.Email) ||
            string.IsNullOrWhiteSpace(_adminSettings.Password))
        {
            _logger.LogWarning(
                "Admin seeding skipped because AdminSettings are incomplete. " +
                "Set AdminSettings:Password with .NET User Secrets before starting the API.");
            return;
        }

        var email = _adminSettings.Email.Trim().ToLowerInvariant();
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (existingUser is not null)
        {
            var userWasUpdated = false;

            if (existingUser.Role != "Admin")
            {
                // Only the email configured by the application owner can be promoted.
                existingUser.Role = "Admin";
                userWasUpdated = true;
            }

            if (_adminSettings.ResetPasswordOnStartup)
            {
                existingUser.PasswordHash = _passwordHasher.HashPassword(
                    existingUser,
                    _adminSettings.Password);
                userWasUpdated = true;

                _logger.LogInformation("Password was reset for configured Admin {Email}.", email);
            }

            if (userWasUpdated)
                await _context.SaveChangesAsync(cancellationToken);

            if (!userWasUpdated)
                _logger.LogInformation("Configured Admin account already exists for {Email}.", email);

            return;
        }

        var admin = new User
        {
            FullName = _adminSettings.FullName.Trim(),
            Email = email,
            Role = "Admin"
        };

        admin.PasswordHash = _passwordHasher.HashPassword(admin, _adminSettings.Password);

        _context.Users.Add(admin);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Initial Admin account was created for {Email}.", email);
    }
}
