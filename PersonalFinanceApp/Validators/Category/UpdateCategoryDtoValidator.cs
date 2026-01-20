using FluentValidation;
using PersonalFinanceApp.DTOs;

namespace PersonalFinanceApp.Validators.Category
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da categoria não pode exceder 100 caracteres.")
                .When(x => x.Name is not null);

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("A descrição da categoria não pode exceder 255 caracteres.");
        }
    }
}
