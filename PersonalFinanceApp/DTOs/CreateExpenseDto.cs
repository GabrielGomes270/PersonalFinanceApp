using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.DTOs
{
    public class CreateExpenseDto
    {
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
