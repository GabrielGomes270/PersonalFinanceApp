namespace PersonalFinanceApp.DTOs.Sumaries
{
    public class MonthlySummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CategorySummaryDto> ByCategory { get; set; } = new();
    }
}
