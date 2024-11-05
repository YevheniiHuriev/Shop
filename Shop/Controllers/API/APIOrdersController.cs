using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class APIOrdersController : ControllerBase
    {
        private readonly IServiceOrder _serviceOrders;
        private readonly ILogger<APIOrdersController> _logger;

        public APIOrdersController(IServiceOrder serviceOrders, ILogger<APIOrdersController> logger)
        {
            _serviceOrders = serviceOrders;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            _logger.LogInformation("Fetching all orders.");
            var orders = await _serviceOrders.ReadAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            _logger.LogInformation($"Fetching order with ID: {id}");
            var order = await _serviceOrders.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning($"Order with ID {id} not found.");
                return NotFound();
            }
            return Ok(order);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                _logger.LogError("CreateOrder: Order object is null.");
                return BadRequest("Order object is null.");
            }

            _logger.LogInformation("Creating a new order.");
            var orderCreated = await _serviceOrders.CreateAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderCreated.Id }, orderCreated);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (order == null)
            {
                _logger.LogError("UpdateOrder: Order object is null.");
                return BadRequest("Order object is null.");
            }

            _logger.LogInformation($"Updating order with ID: {id}");
            var orderUpdated = await _serviceOrders.UpdateAsync(id, order);
            if (orderUpdated == null)
            {
                _logger.LogWarning($"Order with ID {id} not found for update.");
                return NotFound();
            }
            return Ok(orderUpdated);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            _logger.LogInformation($"Deleting order with ID: {id}");
            var deleted = await _serviceOrders.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning($"Order with ID {id} not found for deletion.");
                return NotFound();
            }
            return Ok(new { message = "Order deleted successfully." });
        }
    }
}
