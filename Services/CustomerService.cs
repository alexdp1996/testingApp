using FluentValidation;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;

namespace Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IValidator<CustomerRequest> _validator;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IValidator<CustomerRequest> validator, ICustomerRepository customerRepository) 
        {
            _validator = validator;
            _customerRepository = customerRepository;
        }
        
        public async Task<CustomerResponse> CreateAsync(CustomerRequest request)
        {
            _validator.ValidateAndThrow(request);

            var customer = new Customer
            {
                Name = request.Name,
                LastUpdated = DateTime.UtcNow,
            };

            return Map(await _customerRepository.CreateAsync(customer));
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
        {
            var customers = await _customerRepository.ReadAllAsync();
            return customers.Select(Map);
        }

        private static CustomerResponse Map(Customer customer)
            => new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                OrderCount = customer.OrderCount,
            };
    }
}
