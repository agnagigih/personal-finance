using Personal.Finance.Api.DTOs.Account;

namespace Personal.Finance.Api.Services.Account
{
    public interface IAccountService
    {
        Task<AccountResponse> CreateAccountAsync(Guid userId, CreateAccountRequest request);
        Task<List<AccountResponse>> GetAllAccountAsync(Guid userId);
    }
}
