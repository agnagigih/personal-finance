using Personal.Finance.Api.Models;

namespace Personal.Finance.Api.DTOs.Account
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Balance { get; set; }
        public AccountType Type { get; set; }
    }
}
