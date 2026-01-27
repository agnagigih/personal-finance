using Finance.Api.Data;
using Finance.Api.DTOs.Transaction;
using Finance.Api.Models;
using Finance.Api.Services.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Finance.Api.Responses;

namespace Finance.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService service)
        {
            _transactionService = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
        {
            var transaction = await _transactionService.CreateTransactionAsync(
                UserId,
                request.AccountId,
                request.CategoryId,
                request.Amount,
                request.Type,
                request.Note,
                transactionDate: request.TransactionDate
                );

            return Ok(ApiResponse<object>.Ok(new
            {
                transaction.Id,
                transaction.Account,
                transaction.TransactionDate,
                transaction.CreatedAt
            }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transactionService.GetAllTransactionAsync(UserId);

            return Ok(ApiResponse<object>.Ok(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTransactionRequest request)
        {
            await _transactionService.UpdateAsync(UserId, id, request);

            return Ok(ApiResponse<object>.Ok(new { message = "Data Transaction has been updated." }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _transactionService.DeleteAsync(UserId, id);
                
            return Ok(ApiResponse<object>.Ok(new { message = "Data Transaction has been deleted." }));

        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccount(Guid accountId)
        {
            var result = await _transactionService.GetByAccountAsync(UserId, accountId);

            return Ok(ApiResponse<object>.Ok(result));
        }
    }
}
