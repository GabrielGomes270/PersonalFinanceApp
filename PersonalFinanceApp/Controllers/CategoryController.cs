using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Domain.Entities;
using PersonalFinanceApp.DTOs;
using PersonalFinanceApp.Extensions;
using PersonalFinanceApp.Repositories.Interfaces;

namespace PersonalFinanceApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var userId = User.GetUserId();
            var categories = await _categoryRepository.GetAllCategoriesAsync(userId);

            return Ok(categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var userId = User.GetUserId();

            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);
            if (category == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            return Ok(new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
        {
            var userId = User.GetUserId();

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };

            await _categoryRepository.AddCategoryAsync(category);

            _logger.LogInformation("Usuário {UserId} criou a categoria {CategoryName}",
            userId,
            dto.Name);

            return CreatedAtAction(
            nameof(GetCategoryById),
            new { id = category.Id },
            new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto dto)
        {
            var userId = User.GetUserId();

            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);

            if (category == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _categoryRepository.UpdateCategoryAsync(category);

            _logger.LogInformation("Usuário {UserId} atualizou a categoria {CategoryId}",
            userId,
            id);

            return Ok(new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = User.GetUserId();

            var category = await _categoryRepository.GetCategoryByIdAsync(id, userId);

            if (category == null)
            {
                return NotFound(new { message = "Categoria não encontrada." });
            }

            await _categoryRepository.DeleteCategoryAsync(category);

            _logger.LogWarning("Usuário {UserId} deletou a categoria {CategoryId}",
            userId,
            id);

            return NoContent();
        }
    }
}
