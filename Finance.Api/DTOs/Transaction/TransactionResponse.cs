using Personal.Finance.Api.Models;

namespace Personal.Finance.Api.DTOs.Transaction
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string? AccountName { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName {  get; set; } 
        public decimal Amount { get; set; }
        public TransactionType? Type { get; set; }
        public string? TypeName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
    }
}
