using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderProductsController : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public OrderProductsController(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderProducts()
        {
            var orderProducts = await _shopContext.OrderProducts
                                      .Include(op => op.Order)
                                      .Include(op => op.Product)
                                      .ToListAsync();
            return Ok(orderProducts);
        }

        [HttpGet("{orderId}/{productId}")]
        public async Task<IActionResult> GetOrderProductById(int orderId, int productId)
        {
            var orderProduct = await _shopContext.OrderProducts
                                     .Include(op => op.Order)
                                     .Include(op => op.Product)
                                     .FirstOrDefaultAsync(op => op.OrderId == orderId && op.ProductId == productId);
            if (orderProduct == null) return NotFound("OrderProduct not found.");
            return Ok(orderProduct);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderProduct([FromBody] OrderProduct orderProduct)
        {
            _shopContext.OrderProducts.Add(orderProduct);
            await _shopContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrderProductById), new { orderId = orderProduct.OrderId, productId = orderProduct.ProductId }, orderProduct);
        }

        [HttpPut("{orderId}/{productId}")]
        public async Task<IActionResult> UpdateOrderProduct(int orderId, int productId, [FromBody] OrderProduct updatedOrderProduct)
        {
            if (orderId != updatedOrderProduct.OrderId || productId != updatedOrderProduct.ProductId)
                return BadRequest("OrderProductId not found.");

            var orderProduct = await _shopContext.OrderProducts.FindAsync(orderId, productId);
            if (orderProduct == null) return NotFound("OrderProduct not found.");

            orderProduct.Quantity = updatedOrderProduct.Quantity;
            _shopContext.Entry(orderProduct).State = EntityState.Modified;

            await _shopContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> DeleteOrderProduct(int orderId, int productId)
        {
            var orderProduct = await _shopContext.OrderProducts.FindAsync(orderId, productId);
            if (orderProduct == null) return NotFound("OrderProduct not found.");

            _shopContext.OrderProducts.Remove(orderProduct);
            await _shopContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
