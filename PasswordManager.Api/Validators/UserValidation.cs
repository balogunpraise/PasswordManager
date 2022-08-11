using FluentValidation;
using PasswordManager.Api.Dtos;

namespace PasswordManager.Api.Validators
{
    public class UserValidation : AbstractValidator<RegisterDto>
    {
        public UserValidation()
        {
            RuleFor(p => p.FirstName).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(p => p.LastName).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Please enter a valid email");
            RuleFor(p => p.Password).MinimumLength(8).WithMessage("Password cannot be less than 8 characters");
        }
    }
}
