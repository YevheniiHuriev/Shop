using Shop.Models;

namespace Shop.Services
{
    public interface IServiceProduct
    {
        public Task<Product?> CreateAsync(Product? product);
        public Task<Product?> UpdateAsync(int id, Product? product);
        public Task<bool> DeleteAsync(int id);
        public Task<IEnumerable<Product>?> ReadAsync();
        public Task<Product?> GetByIdAsync(int id);
    }
}