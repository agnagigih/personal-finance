namespace Finance.Api.DTOs.Report
{
    public class IncomeExpenseByCategory
    {
        public string Category { get; set; } = null!;
        public decimal Amount { get; set; }
    }
    public class ReportResponse
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Nett { get; set; }
        public List<IncomeExpenseByCategory>? IncomeByCategory { get; set; }
        public List<IncomeExpenseByCategory>? ExpenseByCategory { get; set; }
    }
}
