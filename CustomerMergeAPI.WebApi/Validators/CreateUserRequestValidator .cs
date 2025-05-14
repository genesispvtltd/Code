using CustomerMergeAPI.Domain.Models;
using FluentValidation;

namespace CustomerMergeAPI.WebApi.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters");

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("At least one role must be assigned");
    }
}
