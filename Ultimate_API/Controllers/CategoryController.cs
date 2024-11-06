using Microsoft.AspNetCore.Mvc;
using Ultimate_API.Dto;
using Ultimate_API.Models;
using Ultimate_API.RepositoryPattern.UnitOfWork;

namespace Ultimate_API.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Map the CategoryDto to the Category entity , we can use auto mapper 
            var category = new Category
            {
                CategoryName = categoryDto.Name  
            };

            await _unitOfWork.Repository<Category, int>().AddAsync(category);

            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(CreateCategory), new { id = category.CategoryId }, new CategoryDto
            {
                Id = category.CategoryId,  //=> generate automatic
                Name = category.CategoryName //=> from user
            });
        }

        [HttpGet("GetCategoryOnly")]
        public async Task<IActionResult> GetCategoryOnly()
        {
            var categories = await _unitOfWork.Repository<Category, int>().GetAllAsync();
                

            if (categories == null || !categories.Any())
            {
                return NotFound();
            }

            var categoryViewModels = categories.Select(c => new
            {
                c.CategoryId,
                c.CategoryName,
               
            }).ToList();

            return Ok(categoryViewModels);
        }

        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _unitOfWork.Repository<Category, int>()
                .GetAllIncludingAsync(
                    category => category.Products // Include Products navigation property
                );

            if (categories == null || !categories.Any())
            {
                return NotFound(); 
            }

            var categoryViewModels = categories.Select(c => new
            {
                c.CategoryId,
                c.CategoryName,
                Products = c.Products.Select(p => new
                {
                    p.ProductId, 
                    p.ProductName, 
                }).ToList()
            }).ToList();

            return Ok(categoryViewModels);
        }


        // GET: api/category/{id}
        [HttpGet("GetCategory/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (id != category.CategoryId) return BadRequest();
            _unitOfWork.Repository<Category , int>().UpdateAsync(category);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Repository<Category, int>().GetByIdAsync(id);
            if (category == null) return NotFound();
            _unitOfWork.Repository<Category, int>().DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
