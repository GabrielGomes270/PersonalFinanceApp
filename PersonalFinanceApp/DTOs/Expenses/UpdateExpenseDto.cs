namespace PersonalFinanceApp.DTOs.Expenses
{
    public class UpdateExpenseDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public int? CategoryId { get; set; }
    }
}
