using FluentValidation;
using PasswordManager.Api.Dtos;

namespace PasswordManager.Api.Validators
{
    public class AccountsValidator : AbstractValidator<LoginCredentialDto>
    {
        public AccountsValidator()
        {
            RuleFor(c => c.WebsiteName).NotEmpty().WithMessage("{PropertyName} cannot be empty");
            RuleFor(c => c.WebAddress).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(c => c.Email).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(c => c.Password).NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
