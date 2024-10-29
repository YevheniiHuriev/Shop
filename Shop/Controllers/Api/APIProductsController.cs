using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIProductsController : ControllerBase
    {
        private readonly IServiceProduct _serviceProducts;
        private readonly ILogger<APIProductsController> _logger;

        public APIProductsController(IServiceProduct serviceProducts, ILogger<APIProductsController> logger)
        {
            _serviceProducts = serviceProducts;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products.");
            var products = await _serviceProducts.ReadAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation($"Fetching product with ID: {id}");
            var product = await _serviceProducts.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError("CreateProduct: Product object is null.");
                return BadRequest("Product object is null.");
            }

            _logger.LogInformation("Creating a new product.");
            var productCreated = await _serviceProducts.CreateAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = productCreated.Id }, productCreated);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError("UpdateProduct: Product object is null.");
                return BadRequest("Product object is null.");
            }

            _logger.LogInformation($"Updating product with ID: {id}");
            var productUpdated = await _serviceProducts.UpdateAsync(id, product);
            if (productUpdated == null)
            {
                _logger.LogWarning($"Product with ID {id} not found for update.");
                return NotFound();
            }
            return Ok(productUpdated);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Deleting product with ID: {id}");
            var deleted = await _serviceProducts.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning($"Product with ID {id} not found for deletion.");
                return NotFound();
            }
            return Ok(new { message = "Product deleted successfully." });
        }
    }
}
