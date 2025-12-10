using FluentValidation;
using RecipeCraftApi.Application.DTOs;

namespace RecipeCraftApi.Application.Validators
{
    public class IngredientValidator : AbstractValidator<IngredientDto>
    {
        public IngredientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(1).WithMessage("Name must be at least 1 character");

            RuleFor(x => x.IngredientTypeId)
                .GreaterThan(0).WithMessage("IngredientTypeId must be valid");
        }
    }
}
