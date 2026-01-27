using Finance.Api.DTOs.Auth;
using Microsoft.AspNetCore.Identity.Data;

namespace Finance.Api.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync (RegistrationRequest request);
        Task<string> LoginAsync(DTOs.Auth.LoginRequest request);
    }
}
