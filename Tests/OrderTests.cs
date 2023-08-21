using FluentValidation;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Moq;
using Services;

namespace Tests
{
    public class OrderTests
    {
        private Settings settings = new Settings
        {
            Database = new DatabaseSettings
            {
                ConnectionString = "con_db"
            },
            ServiceBus = new ServiceBusSettings
            {
                CustomerQueueName = "queue",
                PrimaryConnectionString = "con_sb"
            }
        };

        private Mock<IRepository<Order>> repo;
        private Mock<IMessageBroker> broker;
        private Mock<IRepository<Customer>> customerRepository;

        [SetUp]
        public void Setup()
        {
            repo = new Mock<IRepository<Order>>(MockBehavior.Strict);
            broker = new Mock<IMessageBroker>(MockBehavior.Strict);
            customerRepository = new Mock<IRepository<Customer>>(MockBehavior.Strict);
        }

        [Test]
        public void Order_ValidationFailed()
        {
            var validator = new Mock<IValidator<OrderRequest>>(MockBehavior.Strict);
            validator.Setup(s => s.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException("validation exception"));

            var service = new OrderService(validator.Object, repo.Object, broker.Object, settings, customerRepository.Object);

            var request = new OrderRequest();


            Assert.ThrowsAsync<ValidationException>(async () => await service.CreateAsync(request));
            broker.Verify(b => b.SendAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            repo.Verify(b => b.CreateAsync(It.IsAny<Order>()), Times.Never);
        }

        [Test]
        public async Task Order_ValidationPassed()
        {
            var guid = Guid.NewGuid();

            var validator = new Mock<IValidator<OrderRequest>>(MockBehavior.Strict);
            validator.Setup(s => s.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult { });

            repo
                .Setup(s => s.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(new Order());

            broker
                .Setup(s => s.SendAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var service = new OrderService(validator.Object, repo.Object, broker.Object, settings, customerRepository.Object);


            var request = new OrderRequest { CustomerId = guid };
            await service.CreateAsync(request);

            broker.Verify(b => b.SendAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            repo.Verify(b => b.CreateAsync(It.IsAny<Order>()), Times.Once);
        }
    }
}