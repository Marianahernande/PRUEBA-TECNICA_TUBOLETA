using FluentValidation;

namespace ECommerce.Application.Auth;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Role)
            .Must(r => string.IsNullOrWhiteSpace(r) ||
                       r.Equals("User",  StringComparison.OrdinalIgnoreCase) ||
                       r.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Role debe ser 'User' o 'Admin' (o vacío).");
    }
}
