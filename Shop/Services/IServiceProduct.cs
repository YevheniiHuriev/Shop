using Shop.Models;

namespace Shop.Services
{
    public interface IServiceProduct
    {
        public Task<Product?> Create(Product? product);
        public Task<Product?> Update(Product? product);
        public Task<bool> Delete(int id);
        public Task<IEnumerable<Product>?> Read();
        public Task<Product?> GetProductById(int id);
    }
}
