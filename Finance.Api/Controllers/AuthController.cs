using Finance.Api.Data;
using Finance.Api.DTOs.Auth;
using Finance.Api.Models;
using Finance.Api.Services.Auth;
using Finance.Api.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Finance.Api.Responses;

namespace Finance.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            await _authService.RegisterAsync(request);

            return Ok(ApiResponse<object>.Ok(new { message = "User has succesfully created"}));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(DTOs.Auth.LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);

            return Ok(ApiResponse<object>.Ok(new { token }));         
        }
    }
}
