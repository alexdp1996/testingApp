using FluentValidation;
using Infrastructure.Constants;
using Infrastructure.Dtos;

namespace Infrastructure.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.Name)
                .NotEmpty()
                .MinimumLength(NameLenghtConstants.Min)
                .MaximumLength(NameLenghtConstants.Max);
        }
    }
}
