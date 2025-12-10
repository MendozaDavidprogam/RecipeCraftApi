using FluentValidation;
using RecipeCraftApi.Application.DTOs;

namespace RecipeCraftApi.Application.Validators
{
    public class IngredientTypeValidator : AbstractValidator<IngredientTypeDto>
    {
        public IngredientTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters");
        }
    }
}
