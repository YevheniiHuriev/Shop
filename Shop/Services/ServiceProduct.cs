using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Services
{
    public class ServiceProduct : IServiceProduct
    {
        private readonly ProductContext _productContext;
        private readonly ILogger<ServiceProduct> _logger;
        public ServiceProduct(ProductContext productContext, ILogger<ServiceProduct> logger)
        {
            _productContext = productContext;
            _logger = logger;
        }

        public async Task<Product?> CreateAsync(Product? product)
        {
            if (product == null)
            {
                _logger.LogWarning("Attempt is product null");
                return null;
            }

            _ = await _productContext.Products.AddAsync(product);
            _ = await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productContext.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }
            _productContext.Products.Remove(product);
            await _productContext.SaveChangesAsync();
            return true;
        }

        public async Task<Product?> GetByIdAsync(int id) => await _productContext.Products.FindAsync(id);

        public async Task<IEnumerable<Product>?> ReadAsync() => await _productContext.Products.ToListAsync();

        public async Task<Product?> UpdateAsync(int id, Product? product)
        {
            if (product == null || id != product.Id)
            {
                _logger.LogWarning("Attempt is update product or id [null]");
                return null;
            }

            try
            {
                _ = _productContext.Products.Update(product);
                await _productContext.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
