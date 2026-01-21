namespace PersonalFinanceApp.DTOs
{
    public class AnnualSummaryDto
    {
        public int Year { get; set; }
        public decimal TotalAmount{ get; set; }
        public List<MonthlyTotalDto> ByMonth{ get; set; } = new();
    }
}
