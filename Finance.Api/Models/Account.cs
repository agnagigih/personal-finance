namespace Finance.Api.Models;

public enum AccountType
{
    Cash = 1,
    Bank = 2,
    Ewallet = 3
}

public class Account
{
    public Guid Id {  get; set; }
    public Guid UserId {  get; set; }
    public string Name { get; set; } = null!;
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedAt { get; set;} = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
