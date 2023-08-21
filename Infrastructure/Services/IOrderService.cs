using Infrastructure.Dtos;

namespace Infrastructure.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<OrderResponse> GetAsync(Guid id);
        Task<OrderResponse> CreateAsync(OrderRequest request);
        Task<OrderResponse> MarkAsCompletedAsync(Guid id);
    }
}
