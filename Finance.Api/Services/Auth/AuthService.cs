using Finance.Api.Data;
using Finance.Api.DTOs.Auth;
using Finance.Api.Models;
using Finance.Api.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Personal.Finance.Api.DTOs.Auth;
using Personal.Finance.Api.Exceptions;
using Personal.Finance.Api.Models;
using System.Security.Cryptography;

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
        public async Task<AuthResponse> LoginAsync(DTOs.Auth.LoginRequest request)
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

            var accessToken = _jwt.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            var refreshEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = accessToken,
                ExpiredAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
            };

            _context.RefreshTokens.Add(refreshEntity);
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token == null)
                throw new UnauthorizedActionException("Invalid refresh token");

            if (token.IsRevoked)
                throw new UnauthorizedActionException("Invalid token revoked");
            if (token.ExpiredAt < DateTime.UtcNow)
                throw new UnauthorizedActionException("Invalid token expired");

            token.IsRevoked = true;

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = token.UserId,
                Token = Guid.NewGuid().ToString("N"),
                ExpiredAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
            };

            _context.RefreshTokens.Add(newRefreshToken);


            var accessToken = _jwt.GenerateToken(token.User);

            await _context.SaveChangesAsync();

            return new AuthResponse 
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
        public async Task LogoutAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t  => t.Token == refreshToken);

            if (token == null)
                return;

            token.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }
}
