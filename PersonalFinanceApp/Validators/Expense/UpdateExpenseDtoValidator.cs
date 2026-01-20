using FluentValidation;
using PersonalFinanceApp.DTOs;

namespace PersonalFinanceApp.Validators.Expense
{
    public class UpdateExpenseDtoValidator
    : AbstractValidator<UpdateExpenseDto>
    {
        public UpdateExpenseDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(150).WithMessage("A descrição da despesa não pode exceder 150 caracteres.")
                .When(x => x.Description != null);

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .When(x => x.Amount.HasValue);

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.Date.HasValue);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .When(x => x.CategoryId.HasValue);
        }
    }
}
