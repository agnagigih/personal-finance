using Finance.Api.DTOs.Account;
using Finance.Api.DTOs.Category;

namespace Finance.Api.Services.Account
{
    public interface IAccountService
    {
        Task<AccountResponse> CreateAccountAsync(Guid userId, CreateAccountRequest request);
        Task<List<AccountResponse>> GetAllAccountAsync(Guid userId);
    }
}
