using MediCoreX.Api.Models;
using System.Security.Claims;
using MediCoreX.Api.Services;
using Microsoft.Extensions.Configuration;

namespace MediCoreX.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = "ThisIsASecretKeyForTesting123456789",
                    ["Jwt:Issuer"] = "MediCoreX",
                    ["Jwt:Audience"] = "MediCoreXUsers",
                    ["Jwt:ExpiryMinutes"] = "30"
                })
                .Build();

            _tokenService = new TokenService(config);
        }

        [Fact]
        public void GenerateRefreshToken_ShouldReturnToken()
        {
            // Act
            var token = _tokenService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
public void GenerateRefreshToken_ShouldGenerateDifferentTokens()
{
    // Act
    var token1 = _tokenService.GenerateRefreshToken();
    var token2 = _tokenService.GenerateRefreshToken();

    // Assert
    Assert.NotEqual(token1, token2);
}

[Fact]
public void CreateToken_ShouldReturnValidJwt()
{
    // Arrange
    var user = new User
    {
        Id = 1,
        Email = "rahul@gmail.com",
        Role = "User"
    };

    // Act
    var token = _tokenService.CreateToken(user);

    // Assert
    Assert.NotNull(token);
    Assert.NotEmpty(token);

    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

    Assert.True(handler.CanReadToken(token));
}

[Fact]
public void CreateToken_ShouldContainCorrectClaims()
{
    // Arrange
    var user = new User
    {
        Id = 1,
        Email = "rahul@gmail.com",
        Role = "Admin"
    };

    // Act
    var tokenString = _tokenService.CreateToken(user);

    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var token = handler.ReadJwtToken(tokenString);

    // Assert
    Assert.Equal("1", token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
    Assert.Equal("rahul@gmail.com", token.Claims.First(x => x.Type == ClaimTypes.Email).Value);
    Assert.Equal("Admin", token.Claims.First(x => x.Type == ClaimTypes.Role).Value);
}

[Fact]
public void CreateToken_ShouldHaveCorrectExpiry()
{
    // Arrange
    var user = new User
    {
        Id = 1,
        Email = "rahul@gmail.com",
        Role = "User"
    };

    var before = DateTime.UtcNow;

    // Act
    var tokenString = _tokenService.CreateToken(user);

    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var token = handler.ReadJwtToken(tokenString);

    var after = DateTime.UtcNow;

    // Assert
    Assert.True(token.ValidTo >= before.AddMinutes(29));
    Assert.True(token.ValidTo <= after.AddMinutes(31));
}


    }
}