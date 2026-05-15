using FluentValidation;

namespace Kamirion.RepairShop.Identity.Application.Commands.Login;

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Login_Email_Required_Validation")
            .EmailAddress().WithMessage("Login_Email_Format_Validation");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Login_Password_Required_Validation")
            .MinimumLength(8).WithMessage("Login_Password_MinLength_Validation");
    }
}
