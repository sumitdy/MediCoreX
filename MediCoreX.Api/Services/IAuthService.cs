using MediCoreX.Api.DTOs;

namespace MediCoreX.Api.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
