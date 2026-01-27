using Finance.Api.DTOs.Transaction;
using Finance.Api.Models;

namespace Finance.Api.Services.Transactions
{
    public interface ITransactionService
    {
        Task<Models.Transaction> CreateTransactionAsync(
            Guid userId,
            Guid accountId,
            Guid categoryId,
            decimal amount,
            TransactionType type,
            string note,
            DateTime transactionDate);


        Task<List<TransactionResponse>> GetAllTransactionAsync(Guid userId);

        Task<List<TransactionResponse>> GetByAccountAsync(Guid userId, Guid accountId);

        Task UpdateAsync(Guid userId, Guid transactionId, UpdateTransactionRequest request);

        Task DeleteAsync(Guid userId, Guid transactionId);
    }
}
