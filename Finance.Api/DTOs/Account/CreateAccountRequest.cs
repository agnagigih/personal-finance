using Personal.Finance.Api.Models;

namespace Personal.Finance.Api.DTOs.Account
{
    public class CreateAccountRequest
    {
        public string Name { get; set; } = string.Empty;
        public AccountType Type { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
