using FluentValidation;
using Infrastructure.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IValidator<OrderRequest> _validator;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly string _notificationQueueName;
        private readonly IRepository<Customer> _customerRepository;

        public OrderService(IValidator<OrderRequest> validator, IRepository<Order> orderRepository, IMessageBroker messageBroker, Settings settings, IRepository<Customer> customerRepository)
        {
            _validator = validator;
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _notificationQueueName = settings.ServiceBus.CustomerQueueName;
            _customerRepository = customerRepository;
        }

        public async Task<OrderResponse> CreateAsync(OrderRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);

            var order = new Order
            {
                CustomerId = request.CustomerId,
                Name = request.Name,
                Status = Infrastructure.Enums.OrderStatus.Awaiting
            };

            await _messageBroker.SendAsync(request.CustomerId.ToString(), _notificationQueueName);

            return Map(await _orderRepository.CreateAsync(order));
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync()
        {
            var orders = await _orderRepository.ReadAllAsync();
            return orders.Select(Map);
        }

        public async Task<OrderResponse> GetAsync(Guid id)
        {
            var order = await _orderRepository.ReadAsync(id);
            if (order == null)
            {
                return null;
            }
            return Map(order);
        }

        public async Task<OrderResponse> MarkAsCompletedAsync(Guid id)
        {
            var order = await _orderRepository.ReadAsync(id);
            if (order == null || order.Status == Infrastructure.Enums.OrderStatus.Processed) 
            {
                return null;
            }

            order.Status = Infrastructure.Enums.OrderStatus.Processed;
            var updatedOrder = await _orderRepository.UpdateAsync(order);

            var customer = await _customerRepository.ReadAsync(order.CustomerId);
            var allOrders = await _orderRepository.ReadAllAsync();

            // I suspect that request might come simultaneously
            customer.OrderCount = allOrders.Where(o => o.CustomerId == customer.Id && o.Status == Infrastructure.Enums.OrderStatus.Awaiting).LongCount();
            await _customerRepository.UpdateAsync(customer);
            
            return Map(updatedOrder);
        }

        private OrderResponse Map(Order order)
            => new OrderResponse
            { 
                CustomerId = order.CustomerId,
                Name = order.Name,
                Id = order.Id 
            };
    }
}
