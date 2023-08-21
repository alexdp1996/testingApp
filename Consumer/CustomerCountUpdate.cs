using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class CustomerCountUpdate
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Order> _orderRepository;

        public CustomerCountUpdate(IRepository<Customer> customerRepository, IRepository<Order> orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        [FunctionName("Function1")]
        public async Task Run([ServiceBusTrigger("CustomerCountUpdate", Connection = "PrimaryConnectionString")] Guid id, ILogger log)
        {
            var customer = await _customerRepository.ReadAsync(id);
            // I suspect that request might come simultaneously
            customer.OrderCount = (await _orderRepository.ReadAllAsync()).Select(o => o.CustomerId == id && o.Status == Infrastructure.Enums.OrderStatus.Awaiting).LongCount();
            await _customerRepository.UpdateAsync(customer);
        }
    }
}
