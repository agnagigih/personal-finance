using Finance.Api.Data;
using Finance.Api.DTOs.Account;
using Finance.Api.DTOs.Category;
using Finance.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Finance.Api.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly FinanceDbContext _context;
        public AccountService(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<AccountResponse> CreateAccountAsync(Guid userId, CreateAccountRequest request)
        {
            var account = new Models.Account
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                Type = request.Type,
                Balance = request.InitialBalance,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var result = new AccountResponse()
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance,
                Type = account.Type
            };

            return result;
        }
        public async Task<List<AccountResponse>> GetAllAccountAsync(Guid userId)
        {
            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .Select(a => new AccountResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Balance = a.Balance,
                    Type = a.Type
                }).ToListAsync();
            return accounts;
        }
    }
}
