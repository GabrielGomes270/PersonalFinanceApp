using FluentValidation;
using PersonalFinanceApp.DTOs;

namespace PersonalFinanceApp.Validators.Expense
{
    public class CreateExpenseDtoValidator : AbstractValidator<CreateExpenseDto>
    {
        public CreateExpenseDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(150).WithMessage("A descrição não pode exceder 150 caracteres.");
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("O valor precisa ser maior do que 0.");
            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data não pode ser futura.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("O CategoryId precisa ser um número inteiro positivo.");
        }
    }   
}
