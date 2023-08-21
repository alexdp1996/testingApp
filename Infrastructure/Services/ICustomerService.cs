using Infrastructure.Dtos;

namespace Infrastructure.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponse>> GetAllAsync();
        Task<CustomerResponse> CreateAsync(CustomerRequest request);
    }
}
