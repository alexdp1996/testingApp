using Azure.Messaging.ServiceBus;
using Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker;

namespace Consumer
{
    public class CustomerCountUpdate
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCountUpdate(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [Function(nameof(CustomerCountUpdate))]
        public async Task Run([ServiceBusTrigger("%ServiceBusQueue%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message)
        {
            var id = new Guid(message.Body.ToString());
            const long increase = 1;
            await _customerRepository.UpdateCountAsync(id, increase);
        }
    }
}
