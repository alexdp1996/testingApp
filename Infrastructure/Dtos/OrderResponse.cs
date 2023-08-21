using Infrastructure.Enums;

namespace Infrastructure.Dtos
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
