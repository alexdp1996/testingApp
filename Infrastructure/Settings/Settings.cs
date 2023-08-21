namespace Infrastructure.Settings
{
    public class Settings
    {
        public DatabaseSettings Database { get; set; }
        public ServiceBusSettings ServiceBus { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }

    public class ServiceBusSettings
    {
        public string PrimaryConnectionString { get; set; }
        public string CustomerQueueName { get; set; }
    }
}
