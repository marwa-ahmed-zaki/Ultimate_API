using Microsoft.AspNetCore.Mvc;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.UnitOfWork;

namespace Ultimate_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product , int>().GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // GET: api/product/{id}
        [HttpGet("GetProductOfCategory/{name}")]
        public async Task<IActionResult> GetProductOfCategory(string name)
        {
            var categories = await _unitOfWork.Repository<Category, int>()
     .GetAllIncludingAsync(c => c.Products);   //  include Products

            var category = categories
                .Where(c => c.CategoryName == name)           // Filter by CategoryId 
                .FirstOrDefault();

            if (category == null) return NotFound();

            var products = category.Products.Select(p => new
            {
                p.ProductId,
                p.ProductName
            }).ToList();


            return Ok(products);
        }


        // POST: api/product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _unitOfWork.Repository<Product , int>().AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // PUT: api/product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId) return BadRequest();

            _unitOfWork.Repository<Product , int>().UpdateAsync(product);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product , int>().GetByIdAsync(id);
            if (product == null) return NotFound();

            _unitOfWork.Repository<Product , int>().DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
