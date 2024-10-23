using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Controllers.Api
{

    [ApiController]
    [Route("api/[controller]")]
    public class APIProductsController : BaseController
    {
        private readonly ProductContext _productContext;

        public APIProductsController(ProductContext productContext)
        {
            _productContext = productContext;
        }

        [HttpGet]
        public async Task<IActionResult> ReadAsync()
        {
            var products = await _productContext.Products.ToListAsync();
            return SendResponse(products, "Products successfully found");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return SendError("Validation error", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
            }

            _productContext.Products.Add(product);
            await _productContext.SaveChangesAsync();

            return SendResponse(product, "Product created successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                return SendError("Product not found");
            }

            _productContext.Products.Remove(product);
            await _productContext.SaveChangesAsync();

            return SendResponse(product, "Product deleted successfully");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                return SendError("Product not found");
            }

            return SendResponse(product, "Product found successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                return SendError("Validation error", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
            }

            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                return SendError("Product not found");
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Description = updatedProduct.Description;

            await _productContext.SaveChangesAsync();

            return SendResponse(product, "Product updated successfully");
        }
    }
}
