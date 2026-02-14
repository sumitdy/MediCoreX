using MediCoreX.Api.Models;

namespace MediCoreX.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
