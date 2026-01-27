using Finance.Api.Models;

namespace Finance.Api.DTOs.Transaction
{
    public class UpdateTransactionRequest
    {
        public Guid CategoryId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
