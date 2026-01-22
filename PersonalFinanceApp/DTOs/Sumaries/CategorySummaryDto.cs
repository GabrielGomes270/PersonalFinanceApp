namespace PersonalFinanceApp.DTOs.Sumaries
{
    public class CategorySummaryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
