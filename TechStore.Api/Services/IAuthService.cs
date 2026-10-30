using TechStore.Api.Models;

namespace TechStore.Api.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto> LoginAsync(UserDto request);

        Task<TokenResponseDto>RefreshTokensAsync(RefreshTokenRequestDto request);


    }
}
