using Finance.Api.Data;
using Finance.Api.DTOs.Report;

namespace Finance.Api.Services.Report
{
    public interface IReportService
    {
        Task<ReportResponse> GetMonthlyReportAsync (Guid userId, int year, int month);
    }
}
