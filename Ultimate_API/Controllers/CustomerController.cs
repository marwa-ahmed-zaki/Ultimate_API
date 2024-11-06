using Microsoft.AspNetCore.Mvc;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.UnitOfWork;

namespace Ultimate_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            var customers = await _unitOfWork.Repository<Customer, int>()
                                              .GetAllIncludingAsync(
                                                  customer => customer.OrderMasters // Include OrderMasters
                                              );

            // Apply a Where condition, for example, on the CustomerId
            customers = customers.Where(c => c.CustomerId > 0).ToList(); // Filter customers based on a condition

            if (customers == null || !customers.Any())
            {
                return NotFound();
            }

            var customerViewModels = customers.Select(c => new
            {
                c.CustomerId,
                c.CustomerName,
                OrderMasters = c.OrderMasters.Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    o.OrderNo,
                    OrderDetails = o.OrderDetails.Select(od => new
                    {
                        od.Product, // Assuming Product is a string or another identifiable property
                        od.Price    // Assuming Price is a decimal
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(customerViewModels); // Return the shaped data
        }




        // GET: api/customer/{id}
        [HttpGet("byid")]
        public async Task<IActionResult> GetCustomerbyid(int id)
        {
            var customer = await _unitOfWork.Repository<Customer, int>().GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // POST: api/customer
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _unitOfWork.Repository<Customer , int>().AddAsync(customer);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        // PUT: api/customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerId) return BadRequest();
            _unitOfWork.Repository<Customer, int>().UpdateAsync(customer);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        // DELETE: api/customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _unitOfWork.Repository<Customer , int>().GetByIdAsync(id);
            if (customer == null) return NotFound();
            _unitOfWork.Repository<Customer , int>().DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
