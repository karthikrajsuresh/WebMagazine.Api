using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;
using WebMagazine.Api.Services;
using Microsoft.Extensions.Logging;

namespace WebMagazine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IMapper mapper, CategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _mapper = mapper;
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(categories));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching categories");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<CategoryDTO>(category));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                var createdCategory = await _categoryService.CreateAsync(category);
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.CategoryID }, _mapper.Map<CategoryDTO>(createdCategory));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new category");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            if (id != categoryDto.CategoryID)
            {
                return BadRequest();
            }

            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                await _categoryService.UpdateAsync(category);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
