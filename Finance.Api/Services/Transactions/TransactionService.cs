using Azure.Core;
using Personal.Finance.Api.Data;
using Personal.Finance.Api.DTOs.Transaction;
using Personal.Finance.Api.Models;
using Microsoft.EntityFrameworkCore;
using Personal.Finance.Api.DTOs.Common;
using Personal.Finance.Api.Exceptions;
using System.Transactions;

namespace Personal.Finance.Api.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly FinanceDbContext _context;

        public TransactionService(FinanceDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Transaction> CreateTransactionAsync(
            Guid userId,
            Guid accountId,
            Guid categoryId,
            decimal amount,
            TransactionType type,
            string note,
            DateTime transactionDate
            )
        {

            #region Input Validation
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);

            if (account == null)
                throw new NotFoundException("Account not found", "ACCOUNT_NOT_FOUND");

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null)
                throw new NotFoundException("Category not found", "CATEGORY_NOT_FOUND");

            if (type == TransactionType.Expense && account.Balance < amount)
                throw new BusinessRuleException("Insufficient balance", "INSUFFICIENT_BALANCE");
            #endregion

            #region create transaction
            var transaction = new Models.Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountId = account.Id,
                CategoryId = category.Id,
                Amount = amount,
                Type = type,
                Note = note,
                TransactionDate = transactionDate,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
            };

            _context.Transactions.Add(transaction);
            #endregion

            #region update account balance
            if (type == TransactionType.Expense)
                account.Balance -= amount;
            else if (type == TransactionType.Income)
                account.Balance += amount;

            account.LastUpdatedAt = DateTime.UtcNow;
            #endregion

            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<PagedResult<TransactionResponse>> GetPagedAsyc(Guid userId, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0 || pageSize > 100) pageSize = 20;

            var query = _context.Transactions
                .Include(t => t.Category)
                .Include(t => t.Account)
                .Where(t => t.UserId == userId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((page - 1)  * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionResponse
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    AccountName = t.Account.Name,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name,
                    Amount = t.Amount,
                    Type = t.Type,
                    TypeName = t.Type.ToString(),
                    TransactionDate = t.TransactionDate,
                    Note = t.Note
                }).ToListAsync();

            return new PagedResult<TransactionResponse> 
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        //public async Task<List<TransactionResponse>> GetAllTransactionAsync(Guid userId)
        //{
        //    var transactions = await _context.Transactions
        //        .Where(t => t.UserId == userId)
        //        .OrderByDescending(t => t.TransactionDate)
        //        .Select(t => new TransactionResponse
        //        {
        //            Id = t.Id,
        //            AccountId = t.AccountId,
        //            CategoryId = t.CategoryId,
        //            Amount = t.Amount,
        //            Type = t.Type,
        //            TransactionDate = t.TransactionDate,
        //            Note = t.Note
        //        }).ToListAsync();
        //    return transactions;
        //}

        public async Task<List<TransactionResponse>> GetByAccountAsync(Guid userId, Guid accountId)
        {
            var accountExist = await _context.Accounts
                .AnyAsync(a => a.Id == accountId && a.UserId == userId);

            if (!accountExist)
                throw new NotFoundException("Account not found", "ACCOUNT_NOT_FOUND");

            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionResponse
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name,
                    Amount = t.Amount,
                    Type = t.Type,
                    TypeName = t.Type.ToString(),
                    Note = t.Note,
                    TransactionDate = t.TransactionDate,
                }).ToListAsync();
        }

        public async Task<TransactionResponse?> GetTransactionById(Guid userId, Guid transactionId)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Id == transactionId && t.UserId == userId)
                .Select(t => new TransactionResponse
                {
                    Id = t.Id,
                    AccountId = t.AccountId,
                    AccountName = t.Account.Name,
                    CategoryId = t.CategoryId,
                    CategoryName = t.Category.Name,
                    Amount = t.Amount,
                    Type = t.Type,
                    TypeName = t.Type.ToString(),
                    Note = t.Note,
                    TransactionDate = t.TransactionDate,
                })
                .FirstOrDefaultAsync();
            return transaction;
        }

        public async Task UpdateAsync(
            Guid userId, 
            Guid transactionId, 
            UpdateTransactionRequest request)
        {
            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var transaction = await _context.Transactions
                    .Include(t => t.Account)
                    .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

                if (transaction == null)
                    throw new KeyNotFoundException("Transaction not found");

                // revert to orignial balance
                if (transaction.Type == TransactionType.Expense)
                    transaction.Account.Balance += transaction.Amount;
                else if (transaction.Type == TransactionType.Income)
                    transaction.Account.Balance -= transaction.Amount;

                // update data transaction
                transaction.Amount = request.Amount;
                transaction.Type = request.Type;
                transaction.CategoryId = request.CategoryId;
                transaction.Note = request.Note;
                transaction.TransactionDate = request.TransactionDate;
                transaction.LastUpdatedAt = DateTime.UtcNow;

                // calculate the new balance
                if (request.Type == TransactionType.Expense)
                    transaction.Account.Balance -= transaction.Amount;
                else if (request.Type == TransactionType.Income)
                    transaction.Account.Balance += transaction.Amount;

                transaction.Account.LastUpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(Guid userId, Guid transactionId)
        {
            await using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var transaction = await _context.Transactions
                    .Include(t => t.Account)
                    .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

                if (transaction == null)
                    throw new NotFoundException("Transaction not found", "TRANSACTION_NOT_FOUND");

                // revert to original balance
                if (transaction.Type == TransactionType.Expense)
                    transaction.Account.Balance += transaction.Amount;
                else if (transaction.Type == TransactionType.Income)
                    transaction.Account.Balance -= transaction.Amount;

                _context.Transactions.Remove(transaction);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
