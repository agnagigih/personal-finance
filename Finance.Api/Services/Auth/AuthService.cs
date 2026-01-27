using Finance.Api.Data;
using Finance.Api.DTOs.Auth;
using Finance.Api.Models;
using Finance.Api.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Personal.Finance.Api.Exceptions;

namespace Finance.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly FinanceDbContext _context;
        private readonly JwtTokenService _jwt;
        private readonly PasswordHasher<User> _passwordHaser;

        public AuthService(FinanceDbContext context, JwtTokenService jwt)
        {
            _context = context;
            _jwt = jwt;
            _passwordHaser = new PasswordHasher<User>();

        }

        public async Task RegisterAsync(RegistrationRequest request)
        {
            var exist = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (exist)
            {
                throw new BusinessRuleException("Email already registered.", "EMAIL_ALREADY_REGISTERED");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHaser.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<string> LoginAsync(DTOs.Auth.LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new AuthenticationException("Invalid credentials");
            }

            var result = _passwordHaser.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
             );

            if (result == PasswordVerificationResult.Failed)
            {
                throw new AuthenticationException("Invalid credentials");
            }

            var token = _jwt.GenerateToken(user);
            return token;
        }
    }
}
