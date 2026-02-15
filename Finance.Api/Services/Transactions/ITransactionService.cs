using Personal.Finance.Api.DTOs.Transaction;
using Personal.Finance.Api.Models;
using Personal.Finance.Api.DTOs.Common;

namespace Personal.Finance.Api.Services.Transactions
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


        Task<PagedResult<TransactionResponse>> GetPagedAsyc(Guid userId, int page, int pageSize);

        //Task<List<TransactionResponse>> GetAllTransactionAsync(Guid userId);

        Task<List<TransactionResponse>> GetByAccountAsync(Guid userId, Guid accountId);

        Task<TransactionResponse?> GetTransactionById(Guid userId, Guid transactionId);

        Task UpdateAsync(Guid userId, Guid transactionId, UpdateTransactionRequest request);

        Task DeleteAsync(Guid userId, Guid transactionId);
    }
}
