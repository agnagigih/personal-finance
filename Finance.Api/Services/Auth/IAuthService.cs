using Personal.Finance.Api.DTOs.Auth;

namespace Personal.Finance.Api.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegistrationRequest request);
        Task<AuthResponse> LoginAsync(DTOs.Auth.LoginRequest request);
        string GenerateRefreshToken();
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
