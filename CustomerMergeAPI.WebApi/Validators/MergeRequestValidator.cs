using CustomerMergeAPI.Domain.Models;
using FluentValidation;

namespace CustomerMergeAPI.WebApi.Validators;

public class MergeRequestValidator : AbstractValidator<MergeRequest>
{
    public MergeRequestValidator()
    {
        RuleFor(x => x.GroupKey)
            .NotEmpty().WithMessage("GroupKey is required");

        RuleFor(x => x.ParentCustCode)
            .NotEmpty().WithMessage("Parent customer code is required");
    }
}
