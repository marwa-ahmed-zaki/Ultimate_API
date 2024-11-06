using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.UnitOfWork;

namespace Ultimate_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UltimateContext _context;



        public EmployeeController(IUnitOfWork unitOfWork , UltimateContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployees()
        {

            var employees = await _context.Employees
                .Include(e => e.OrderMasters)
                    .ThenInclude(o => o.Customer) 
                .Include(e => e.OrderMasters)
                    .ThenInclude(o => o.AccountCodeNavigation) 
                .Select(e => new
                {
                    e.EmployeeId,
                    e.EmployeeName,
                    Orders = e.OrderMasters.Select(o => new
                    {
                        o.OrderId,
                        CustomerName = o.Customer.CustomerName,
                        OrderDate = o.OrderDate,
                        AccountNo = o.AccountCode,
                        AccountName = o.AccountCodeNavigation.AccountName,
                        TotalPrice = o.OrderDetails.Sum(d => d.Quantity * d.Price) // Calculate total price from OrderDetails
                    }).ToList()
                })
                .ToListAsync();

            return Ok(employees);
        }


        //  public async Task<IActionResult> GetAllEmployee()
        //  {
        //      // Retrieve all employees with their related OrderMasters and OrderDetails, and their associated Customer entities.
        //  //    var employees = await _unitOfWork.Repository<Employee, int>()
        //  //.GetAllIncludingAsync(e => e.OrderMasters);

        //      var employees = await _unitOfWork.Repository<Employee, int>()
        // .GetAllIncludingAsync(e => e.OrderMasters); // Include OrderMasters and OrderDetails

        //      var _employees = await _unitOfWork.Repository<OrderMaster, int>()
        //.GetAllIncludingAsync(e => e.OrderDetails);


        //      // Shape the result to project only the necessary properties


        //      return Ok(_employees);
        //  }


        // GET: api/employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _unitOfWork.Repository<Employee, int>().GetByIdAsync(id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        // POST: api/employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _unitOfWork.Repository<Employee, int>().AddAsync(employee);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.EmployeeId) return BadRequest();

            _unitOfWork.Repository<Employee, int>().UpdateAsync(employee);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.Repository<Employee, int>().GetByIdAsync(id);
            if (employee == null) return NotFound();

            _unitOfWork.Repository<Employee , int>().DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
