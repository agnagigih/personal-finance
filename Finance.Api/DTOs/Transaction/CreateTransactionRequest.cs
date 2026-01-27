using Finance.Api.Models;

namespace Finance.Api.DTOs.Transaction
{
    public class CreateTransactionRequest
    {
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
    }
}
