using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdersController : ControllerBase
    {
        private readonly ShopContext _shopContext;
        public OrdersController(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _shopContext.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.OrderProducts)
                                .ThenInclude(op => op.Product)
                                .ToListAsync();

            if (orders != null)
                return Ok(orders);
            else 
                return BadRequest("There are no orders!");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _shopContext.Orders
                              .Include(o => o.Customer)
                              .Include(o => o.OrderProducts)
                              .ThenInclude(op => op.Product)
                              .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound("Order not found.");
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            _shopContext.Orders.Add(order);
            await _shopContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (id != updatedOrder.Id) return BadRequest("OrderId not found.");

            var order = await _shopContext.Orders.FindAsync(id);
            if (order == null) return NotFound("Order not found.");

            order.CreateDate = updatedOrder.CreateDate;
            order.CustomerId = updatedOrder.CustomerId;
            _shopContext.Entry(order).State = EntityState.Modified;

            await _shopContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _shopContext.Orders.FindAsync(id);
            if (order == null) return NotFound("Order not found.");

            _shopContext.Orders.Remove(order);
            await _shopContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
