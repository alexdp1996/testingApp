using Azure.Messaging.ServiceBus;
using Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Consumer
{
    public class CustomerCountUpdate
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerCountUpdate> _logger;

        public CustomerCountUpdate(ICustomerRepository customerRepository, ILogger<CustomerCountUpdate> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        //[Function(nameof(CustomerCountUpdate))]
        //public void Run([ServiceBusTrigger("%ServiceBusQueue%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message)
        //{
        //    _logger.LogInformation("Message ID: {id}", message.MessageId);
        //    _logger.LogInformation("Message Body: {body}", message.Body);
        //    _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
        //}

        [Function(nameof(CustomerCountUpdate))]
        public async Task Run([ServiceBusTrigger("%ServiceBusQueue%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);
            var id = new Guid(message.Body.ToString());
            const long increase = 1;
            await _customerRepository.UpdateCountAsync(id, increase);
        }
    }
}
