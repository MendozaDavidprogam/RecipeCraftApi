using RecipeCraftApi.Application.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace RecipeCraftApi.Application.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must be at least 3 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MinimumLength(2).WithMessage("LastName must be at least 2 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter")
                .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter")
                .Must(ContainDigit).WithMessage("Password must contain at least one number")
                .Must(ContainSpecial).WithMessage("Password must contain at least one special character");
        }

        private bool ContainUppercase(string password) => Regex.IsMatch(password, "[A-Z]");
        private bool ContainLowercase(string password) => Regex.IsMatch(password, "[a-z]");
        private bool ContainDigit(string password) => Regex.IsMatch(password, "[0-9]");
        private bool ContainSpecial(string password) => Regex.IsMatch(password, "[^a-zA-Z0-9]");
    }

    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.LastName)
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Password)
                .MinimumLength(8).When(x => !string.IsNullOrEmpty(x.Password))
                .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter")
                .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter")
                .Must(ContainDigit).WithMessage("Password must contain at least one number")
                .Must(ContainSpecial).WithMessage("Password must contain at least one special character");
        }

        private bool ContainUppercase(string password) => Regex.IsMatch(password, "[A-Z]");
        private bool ContainLowercase(string password) => Regex.IsMatch(password, "[a-z]");
        private bool ContainDigit(string password) => Regex.IsMatch(password, "[0-9]");
        private bool ContainSpecial(string password) => Regex.IsMatch(password, "[^a-zA-Z0-9]");
    }
}
