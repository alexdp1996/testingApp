namespace Infrastructure.Services
{
    public interface IMessageBroker
    {
        Task SendAsync(string message, string queueName);
    }
}
