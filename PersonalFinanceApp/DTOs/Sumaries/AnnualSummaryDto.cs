namespace PersonalFinanceApp.DTOs.Sumaries
{
    public class AnnualSummaryDto : BaseSummaryDto
    {
        public int Year { get; set; }

        public List<MonthlyTotalDto> ByMonth{ get; set; } = new();
    }
}
