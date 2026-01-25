using PersonalFinanceApp.DTOs.Sumaries;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Text;

namespace PersonalFinanceApp.Services
{
    public class ReportExportService : IReportExportService
    {
        public byte[] ExportToCsv(BaseSummaryDto summary, string title)
        {
            var sb = new StringBuilder();

            sb.AppendLine(title);
            sb.AppendLine();

            if (summary is MonthlySummaryDto monthly)
            {
                sb.AppendLine("Categoria,Total");

                foreach (var item in monthly.ByCategory)
                    sb.AppendLine($"{item.CategoryName},{item.Total}");

                sb.AppendLine($"TOTAL,{monthly.TotalAmount}");
            }
            else if (summary is AnnualSummaryDto annual)
            {
                sb.AppendLine("Mês,Total");

                foreach (var month in annual.ByMonth.OrderBy(m => m.Month))
                    sb.AppendLine($"{GetMonthName(month.Month)},{month.Total}");

                sb.AppendLine($"TOTAL,{annual.TotalAmount}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }


        public byte[] ExportToPdf(BaseSummaryDto summary, string title)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text(title)
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(120);
                            });

                            if (summary is MonthlySummaryDto monthly)
                            {
                                table.Header(h =>
                                {
                                    h.Cell().Text("Categoria").Bold();
                                    h.Cell().Text("Total").Bold();
                                });

                                foreach (var item in monthly.ByCategory)
                                {
                                    table.Cell().Text(item.CategoryName);
                                    table.Cell().Text(item.Total.ToString("C"));
                                }

                                table.Footer(f =>
                                {
                                    f.Cell().Text("Total Geral").Bold();
                                    f.Cell().Text(monthly.TotalAmount.ToString("C")).Bold();
                                });
                            }

                            if (summary is AnnualSummaryDto annual)
                            {
                                table.Header(h =>
                                {
                                    h.Cell().Text("Mês").Bold();
                                    h.Cell().Text("Total").Bold();
                                });

                                foreach (var item in annual.ByMonth.OrderBy(m => m.Month))
                                {
                                    table.Cell().Text(GetMonthName(item.Month));
                                    table.Cell().Text(item.Total.ToString("C"));
                                }

                                table.Footer(f =>
                                {
                                    f.Cell().Text("Total Geral").Bold();
                                    f.Cell().Text(annual.TotalAmount.ToString("C")).Bold();
                                });
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x => x.CurrentPageNumber().ToString());
                });
            });

            return document.GeneratePdf();
        }


        private static string GetMonthName(int month)
        {
            return new DateTime(1, month, 1).ToString("MMMM", new CultureInfo("pt-BR"));
        }
    }
}
