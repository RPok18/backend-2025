using Dishapi.Core.Dtos;

namespace Dishapi.BLL.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<bool> UserExistsAsync(string email);
    }
}

