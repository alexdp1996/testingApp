using Infrastructure.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService) 
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerResponse>> GetAll()
            => await _customerService.GetAllAsync();

        [HttpPost]
        public async Task<CustomerResponse> Create(CustomerRequest request)
            => await _customerService.CreateAsync(request);
    }
}
