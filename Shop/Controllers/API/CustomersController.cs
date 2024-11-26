using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersController : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public CustomersController(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _shopContext.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _shopContext.Customers.FindAsync(id);
            if (customer == null) return NotFound("Customer not found.");
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            _shopContext.Customers.Add(customer);
            await _shopContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            if (id != updatedCustomer.Id) return BadRequest("CustomerId not found.");

            var customer = await _shopContext.Customers.FindAsync(id);
            if (customer == null) return NotFound("Customer not found.");

            customer.Name = updatedCustomer.Name;
            _shopContext.Entry(customer).State = EntityState.Modified;

            await _shopContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _shopContext.Customers.FindAsync(id);
            if (customer == null) return NotFound("Customer not found.");

            _shopContext.Customers.Remove(customer);
            await _shopContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
