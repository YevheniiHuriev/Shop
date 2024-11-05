using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Services
{
    public class ServiceOrder : IServiceOrder
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger<ServiceOrder> _logger;

        public ServiceOrder(OrderContext orderContext, ILogger<ServiceOrder> logger)
        {
            _orderContext = orderContext;
            _logger = logger;
        }

        public async Task<Order?> CreateAsync(Order? order)
        {
            if (order == null)
            {
                _logger.LogWarning("Attempt is order null ...");
                return null;
            }

            _ = await _orderContext.Orders.AddAsync(order);
            await _orderContext.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _orderContext.Orders.FindAsync(id);
            if (order == null)
            {
                _logger.LogWarning($"Order with id {id} not found");
                return false;
            }

            _orderContext.Orders.Remove(order);
            await _orderContext.SaveChangesAsync();
            return true;

        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderContext.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>?> ReadAsync()
        {
            return await _orderContext.Orders.ToListAsync();
        }

        public async Task<Order?> UpdateAsync(int id, Order? order)
        {
            if (order == null || id != order.Id)
            {
                _logger.LogWarning("Attempt to update order or id is null");
                return null;
            }

            try
            {
                _orderContext.Orders.Update(order);
                await _orderContext.SaveChangesAsync();
                return order;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

        }
    }
}
