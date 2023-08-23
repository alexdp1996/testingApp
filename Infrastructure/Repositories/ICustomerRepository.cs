using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task UpdateCountAsync(Guid id, long diff);
    }
}
