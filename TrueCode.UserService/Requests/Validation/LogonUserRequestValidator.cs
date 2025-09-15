using FluentValidation;

namespace TrueCode.UserService.Requests.Validation;

public class LogonUserRequestValidator : AbstractValidator<LogonUserRequest>
{
    public LogonUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}