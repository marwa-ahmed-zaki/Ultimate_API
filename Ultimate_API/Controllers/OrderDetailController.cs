using Microsoft.AspNetCore.Mvc;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.UnitOfWork;

namespace Ultimate_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            var orders = await _unitOfWork.Repository<OrderDetail, int>().GetAllAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // GET: api/orderdetail/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var orderDetail = await _unitOfWork.Repository<OrderDetail, int>().GetByIdAsync(id);
            if (orderDetail == null) return NotFound();
            return Ok(orderDetail);
        }

        // POST: api/orderdetail
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetail orderDetail)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _unitOfWork.Repository<OrderDetail, int>().AddAsync(orderDetail);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.OrderDetailsId }, orderDetail);
        }

        // PUT: api/orderdetail/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailsId) return BadRequest();

            _unitOfWork.Repository<OrderDetail , int>().UpdateAsync(orderDetail);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/orderdetail/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _unitOfWork.Repository<OrderDetail, int>().GetByIdAsync(id);
            if (orderDetail == null) return NotFound();

            _unitOfWork.Repository<OrderDetail , int>().DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
