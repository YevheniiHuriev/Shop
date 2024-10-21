using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Services
{
    public class ServiceProduct : IServiceProduct
    {
        private readonly ProductContext _productContext;
        public ServiceProduct(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public async Task<Product?> Create(Product? product)
        {
            if (product == null) return null;

            await _productContext.AddAsync(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> Delete(int id)
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

        public async Task<Product?> GetProductById(int id)
        {
            return await _productContext.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>?> Read()
        {
            return await _productContext.Products.ToListAsync();
        }

        public async Task<Product?> Update(Product? product)
        {
            if (product == null) return null;

            try
            {
                _productContext.Products.Update(product);
                await _productContext.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
