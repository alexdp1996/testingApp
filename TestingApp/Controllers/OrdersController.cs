using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderResponse>> GetAll()
            => await _orderService.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<OrderResponse> Get(Guid id)
            => await _orderService.GetAsync(id);

        [HttpPost()]
        public async Task<OrderResponse> Create(OrderRequest request)
            => await _orderService.CreateAsync(request);


        [HttpPatch("{id}")]
        public async Task<OrderResponse> MarkAsCompleted(Guid id) 
            => await _orderService.MarkAsCompletedAsync(id);
    }
}
