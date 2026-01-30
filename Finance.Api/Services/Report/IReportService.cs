using Personal.Finance.Api.DTOs.Report;

namespace Personal.Finance.Api.Services.Report
{
    public interface IReportService
    {
        Task<ReportResponse> GetMonthlyReportAsync (Guid userId, int year, int month);
    }
}
