using Microsoft.AspNetCore.Mvc;
using Ultimate_API.Models; // Ensure your OrderMaster model is included here
using Ultimate_API.RepositoryPattern.UnitOfWork;
using System.Threading.Tasks;

namespace Ultimate_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderMasterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderMasterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _unitOfWork.Repository<OrderMaster , int>().GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

       
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _unitOfWork.Repository<OrderMaster, int>()
                                   .GetAllIncludingAsync(
                                       order => order.AccountCodeNavigation,   
                                       order => order.OrderDetails,           
                                       order => order.Customer                
                                   );

            var orderViewModels = orders.Select(o => new
            {
                o.OrderId,
                o.OrderDate,
                o.AccountCodeNavigation,
                Customer = new
                {
                    o.Customer.CustomerId,
                    o.Customer.CustomerName
                },
                OrderDetails = o.OrderDetails.Select(d => new
                {
                    d.ProductId,
                    d.Quantity,
                    d.Price
                }).ToList()
            }).ToList();

            return Ok(orderViewModels);


            //if (orders == null || !orders.Any())
            //{
            //    return NotFound();
            //}

            //return Ok(orders);
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderMaster order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _unitOfWork.Repository<OrderMaster , int>().AddAsync(order);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // PUT: api/order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderMaster order)
        {
            if (id != order.OrderId) return BadRequest();

            _unitOfWork.Repository<OrderMaster , int>().UpdateAsync(order);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.Repository<OrderMaster , int>().GetByIdAsync(id);
            if (order == null) return NotFound();

            _unitOfWork.Repository<OrderMaster , int>().DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
