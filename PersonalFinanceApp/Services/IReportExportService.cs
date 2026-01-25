using PersonalFinanceApp.DTOs.Sumaries;

namespace PersonalFinanceApp.Services
{
    public interface IReportExportService
    {
        byte[] ExportToCsv(BaseSummaryDto summary, string title);
        byte[] ExportToPdf(BaseSummaryDto summary, string title);
    }
}
