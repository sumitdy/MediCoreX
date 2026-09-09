using MediCoreX.Api.DTOs;

namespace MediCoreX.Api.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
