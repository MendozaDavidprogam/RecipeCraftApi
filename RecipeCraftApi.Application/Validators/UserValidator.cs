using RecipeCraftApi.Application.DTOs;
using FluentValidation;

namespace RecipeCraftApi.Application.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }

    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name));
            RuleFor(x => x.Password).MinimumLength(6).When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}