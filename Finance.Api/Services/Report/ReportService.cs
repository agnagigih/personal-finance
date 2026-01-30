using Personal.Finance.Api.Data;
using Personal.Finance.Api.DTOs.Report;
using Microsoft.EntityFrameworkCore;

namespace Personal.Finance.Api.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly FinanceDbContext _context;
        public ReportService(FinanceDbContext context) 
        {
            _context = context; 
        }

        public async Task<ReportResponse> GetMonthlyReportAsync(Guid userId,int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            var transaction = await _context.Transactions
                .Where(t =>
                    t.UserId == userId &&
                    t.TransactionDate >= startDate &&
                    t.TransactionDate < endDate)
                .Include(t => t.Category)
                .ToListAsync();

            var income = transaction
                .Where(t => t.Type == Models.TransactionType.Income)
                .Sum(t => t.Amount);

            var expense = transaction
                .Where(t => t.Type == Models.TransactionType.Expense)
                .Sum(t => t.Amount);

            var incomeByCategory = transaction
                .Where(t => t.Type == Models.TransactionType.Income)
                .GroupBy(t => t.Category.Name)
                .Select(g => new IncomeExpenseByCategory
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            var expenseByCategory = transaction
                .Where(t => t.Type == Models.TransactionType.Expense)
                .GroupBy(t => t.Category.Name)
                .Select(g => new IncomeExpenseByCategory
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            var result = new ReportResponse()
            {
                Income = income,
                Expense = expense,
                Nett = income - expense,
                IncomeByCategory = incomeByCategory,
                ExpenseByCategory = expenseByCategory
            };

            return result;
        }
    }
}
