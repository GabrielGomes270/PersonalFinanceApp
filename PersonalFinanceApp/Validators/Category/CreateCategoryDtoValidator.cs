using FluentValidation;
using PersonalFinanceApp.DTOs;

namespace PersonalFinanceApp.Validators.Category
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da categoria não pode exceder 100 caracteres.");
            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("A descrição não pode exceder 255 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
