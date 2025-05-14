using CustomerMergeAPI.Domain.Commands;
using FluentValidation;


namespace CustomerMergeAPI.WebApi.Validators;

public class MergeGroupCommandValidator : AbstractValidator<MergeGroupCommand>
{
    public MergeGroupCommandValidator()
    {
        RuleFor(x => x.GroupKey)
            .NotEmpty().WithMessage("GroupKey is required");

        RuleFor(x => x.ParentCustCode)
            .NotEmpty().WithMessage("ParentCustCode is required");

        RuleFor(x => x.ParentCustomer).NotNull().WithMessage("Parent customer is required");

        When(x => x.ParentCustomer != null, () =>
        {
            RuleFor(x => x.ParentCustomer!.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.ParentCustomer!.Add01)
                .NotEmpty().WithMessage("Add01 is required");

            RuleFor(x => x.ParentCustomer!.Add02)
                .NotEmpty().WithMessage("Add02 is required");

            RuleFor(x => x.ParentCustomer!.PostCode)
                .NotEmpty().WithMessage("PostCode is required");

            RuleFor(x => x.ParentCustomer!.Country)
                .NotEmpty().WithMessage("Country is required");
        });
    }
}
