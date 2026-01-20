using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.DTOs
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
