namespace PersonalFinanceApp.DTOs.Sumaries
{
    public class MonthlySummaryDto : BaseSummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public List<CategorySummaryDto> ByCategory { get; set; } = new();
    }
}