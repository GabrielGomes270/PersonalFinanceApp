using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Extensions;
using PersonalFinanceApp.Repositories.Interfaces;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportExportController : ControllerBase
    {
        private readonly IReportExportService _reportExportService;
        private readonly IExpenseRepository _expenseRepository;

        public ReportExportController(IReportExportService reportExportService, IExpenseRepository expenseRepository)
        {
            _reportExportService = reportExportService;
            _expenseRepository = expenseRepository;
        }

        [HttpGet("monthly/export/pdf")]
        public async Task<IActionResult> ExportMonthlyPdf(int year, int month)
        {
            var userId = User.GetUserId();

            var summary = await _expenseRepository.GetMonthlySummaryAsync(userId, year, month);

            var file = _reportExportService.ExportToPdf(summary, $"Resumo Mensal - {month}/{year}");

            return File(file, "application/pdf", $"Resumo-Mensal-{year}-{month}.pdf");
        }

        [HttpGet("annual/export/pdf")]
        public async Task<IActionResult> ExportAnnualPdf(int year)
        {
            var userId = User.GetUserId();

            var summary = await _expenseRepository
                .GetAnnualSummaryAsync(userId, year);

            var file = _reportExportService.ExportToPdf(summary,$"Resumo Anual - {year}");

            return File(file, "application/pdf", $"Resumo-Anual-{year}.pdf");
        }

        [HttpGet("monthly/export/csv")]
        public async Task<IActionResult> ExportMonthlyCsv(int year, int month)
        {
            var userId = User.GetUserId();

            var summary = await _expenseRepository
                .GetMonthlySummaryAsync(userId, year, month);

            var file = _reportExportService.ExportToCsv(summary,$"Resumo Mensal - {month}/{year}");

            return File(file,"text/csv", $"Resumo-Mensal-{year}-{month}.csv");
        }

        [HttpGet("annual/export/csv")]
        public async Task<IActionResult> ExportAnnualCsv(int year)
        {
            var userId = User.GetUserId();

            var summary = await _expenseRepository
                .GetAnnualSummaryAsync(userId, year);

            var file = _reportExportService.ExportToCsv(summary, $"Resumo Anual - {year}");

            return File(file, "text/csv", $"Resumo-Anual-{year}.csv");
        }
    }
}
