using Infrastructure.Enums;

namespace Infrastructure.Models
{
    public class Order : Base
    {
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
