using Azure.Messaging.ServiceBus;
using Infrastructure.Services;

namespace Services
{
    public class MessageBroker : IMessageBroker
    {
        public readonly ServiceBusClient _client;

        public MessageBroker(ServiceBusClient client)
        {
            _client = client;
        }

        public async Task SendAsync(string message, string queueName)
        {
            var sender = _client.CreateSender(queueName);
            await sender.SendMessageAsync(new ServiceBusMessage(message));
        }
    }
}
