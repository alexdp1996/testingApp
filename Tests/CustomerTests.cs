using FluentValidation;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Moq;
using Services;

namespace Tests
{
    public class CustomerTests
    {
        private Mock<ICustomerRepository> repo;

        [SetUp]
        public void Setup()
        {
            repo = new Mock<ICustomerRepository>(MockBehavior.Strict);
        }

        [Test]
        public void Customer_ValidationFailed()
        {
            var validator = new Mock<IValidator<CustomerRequest>>(MockBehavior.Strict);
            validator.Setup(s => s.Validate(It.IsAny<IValidationContext>()))
                .Throws(new ValidationException("validation exception"));

            var service = new CustomerService(validator.Object, repo.Object);

            var request = new CustomerRequest();


            Assert.ThrowsAsync<ValidationException>(async () => await service.CreateAsync(request));
            repo.Verify(b => b.CreateAsync(It.IsAny<Customer>()), Times.Never);
        }

        [Test]
        public async Task Customer_ValidationPassed()
        {
            var guid = Guid.NewGuid();

            var validator = new Mock<IValidator<CustomerRequest>>(MockBehavior.Strict);
            validator.Setup(s => s.Validate(It.IsAny<IValidationContext>()))
                .Returns(new FluentValidation.Results.ValidationResult { });

            repo
                .Setup(s => s.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(new Customer());

            var service = new CustomerService(validator.Object, repo.Object);


            var request = new CustomerRequest();
            await service.CreateAsync(request);

            repo.Verify(b => b.CreateAsync(It.IsAny<Customer>()), Times.Once);
        }
    }
}