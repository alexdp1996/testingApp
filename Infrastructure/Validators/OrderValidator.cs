using FluentValidation;
using Infrastructure.Constants;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Validators
{
    public class OrderValidator : AbstractValidator<OrderRequest>
    {
        public OrderValidator(IRepository<Customer> customerRepository) 
        {
            ClassLevelCascadeMode = CascadeMode.Continue;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(o => o.Name)
                .NotEmpty()
                .MinimumLength(NameLenghtConstants.Min)
                .MaximumLength(NameLenghtConstants.Max);

            RuleFor(o => o.CustomerId)
                .NotEmpty()
                .MustAsync(async (x, token) =>
                {
                    var customer = await customerRepository.ReadAsync(x);
                    return customer != null;
                })
                .WithMessage("Customer is not present");
        }
    }
}
