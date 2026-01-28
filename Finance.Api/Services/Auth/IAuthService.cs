using Finance.Api.DTOs.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Personal.Finance.Api.DTOs.Auth;

namespace Finance.Api.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegistrationRequest request);
        Task<AuthResponse> LoginAsync(DTOs.Auth.LoginRequest request);
        Task<string> GenerateRefreshToken();
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
