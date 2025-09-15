using FluentValidation;

namespace TrueCode.UserService.Requests.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters")
            .Must(username => username?.All(c => char.IsLetterOrDigit(c) || c == '_') ?? false)
            .WithMessage("Username can only contain letters, numbers and underscores");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(3).WithMessage("Password must be at least 3 characters")
            .Must(username => username?.All(c => char.IsLetterOrDigit(c) || c == '_') ?? false)
            .WithMessage("Username can only contain letters, numbers and underscores");
    }
}