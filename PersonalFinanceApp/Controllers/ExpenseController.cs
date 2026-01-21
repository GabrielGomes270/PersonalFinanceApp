using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Domain.Entities;
using PersonalFinanceApp.DTOs;
using PersonalFinanceApp.Extensions;
using PersonalFinanceApp.Repositories.Interfaces;
using PersonalFinanceApp.Shared;

namespace PersonalFinanceApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ExpenseController> _logger;
        public ExpenseController(IExpenseRepository expenseRepository, 
            ILogger<ExpenseController> logger, 
            ICategoryRepository categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses(
             int? categoryId,
             int? month,
             int page = 1,
             int pageSize = 10,
             string orderBy = "date",
             string direction = "desc")
        {
            var userId = User.GetUserId();

            var result = await _expenseRepository.GetAllExpensesAsync(
                userId,
                categoryId,
                month,
                page,
                pageSize,
                orderBy,
                direction);

            var response = new PagedResultDto<ExpenseResponseDto>
            {
                Items = result.Items.Select(expense => new ExpenseResponseDto
                {
                    Id = expense.Id,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Date = expense.Date,
                    CategoryId = expense.CategoryId,
                    CategoryName = expense.Category.Name
                }),

                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            var userId = User.GetUserId();

            var expense = await _expenseRepository.GetExpenseByIdAsync(id, userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Despesa não encontrada.");
            }

            return Ok(new ExpenseResponseDto
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(CreateExpenseDto dto)
        {
            var userId = User.GetUserId();

            var categoryExists = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId, userId);

            if (categoryExists == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            var expense = new Expense
            {
                Description = dto.Description,
                Amount = dto.Amount,
                Date = dto.Date,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            await _expenseRepository.AddExpenseAsync(expense);


            _logger.LogInformation(
                "Usuário {UserId} criou uma despesa de {Amount}",
                userId,
                dto.Amount);

            return CreatedAtAction(
            nameof(GetExpenseById),
            new { id = expense.Id },
            new ExpenseResponseDto
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                CategoryId = expense.CategoryId,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, UpdateExpenseDto dto)
        {
            var userId = User.GetUserId();

            var expense = await _expenseRepository.GetExpenseByIdAsync(id, userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Despesa não encontrada.");
            }

            var categoryExists = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId ?? expense.CategoryId, userId);

            if (categoryExists == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
                expense.Description = dto.Description;

            if (dto.Amount.HasValue)
                expense.Amount = dto.Amount.Value;

            if (dto.Date.HasValue)
                expense.Date = dto.Date.Value;

            if (dto.CategoryId.HasValue)
                expense.CategoryId = dto.CategoryId.Value;

            await _expenseRepository.UpdateExpenseAsync(expense);

            _logger.LogInformation("Usuário {UserId} atualizou a despesa {ExpenseId}",
            userId,
            id);

            return Ok(new ExpenseResponseDto
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category.Name
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = User.GetUserId();

            var expense = await _expenseRepository.GetExpenseByIdAsync(id, userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Despesa não encontrada.");
            }

            await _expenseRepository.DeleteExpenseAsync(expense);

            _logger.LogWarning("Usuário {UserId} deletou a despesa {ExpenseId}",
            userId,
            id);

            return NoContent();
        }
    }
}
