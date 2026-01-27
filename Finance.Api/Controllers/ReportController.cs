using Finance.Api.Data;
using Finance.Api.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Finance.Api.Responses;

namespace Finance.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/report")]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly([FromQuery] int year, [FromQuery] int month)
        {
            var result = await _reportService.GetMonthlyReportAsync(UserId, year, month);

            return Ok(ApiResponse<object>.Ok(result));
        }
    }
}
