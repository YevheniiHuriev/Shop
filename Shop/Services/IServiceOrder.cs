using Shop.Models;

namespace Shop.Services
{
    public interface IServiceOrder
    {
        public Task<Order?> CreateAsync(Order? order);
        public Task<Order?> UpdateAsync(int id, Order? order);
        public Task<bool> DeleteAsync(int id);
        public Task<IEnumerable<Order>?> ReadAsync();
        public Task<Order?> GetByIdAsync(int id);
    }
}
