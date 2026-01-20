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
        private readonly ILogger<ExpenseController> _logger;
        public ExpenseController(IExpenseRepository expenseRepository, ILogger<ExpenseController> logger)
        {
            _expenseRepository = expenseRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses(
             int? categoryId,
             int? month,
             int page = 1,
             int pageSize = 10)
        {
            var userId = User.GetUserId();

            var expenses = await _expenseRepository.GetAllExpensesAsync(
                userId,
                categoryId,
                month,
                page,
                pageSize);

            return Ok(expenses.Select(e => new ExpenseResponseDto
            {
                Id = e.Id,
                Description = e.Description,
                Amount = e.Amount,
                Date = e.Date,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.Name
            }));
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
                CategoryName = expense.Category.Name
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, CreateExpenseDto dto)
        {
            var userId = User.GetUserId();

            var expense = await _expenseRepository.GetExpenseByIdAsync(id, userId);

            if (expense == null)
            {
                throw new KeyNotFoundException("Despesa não encontrada.");
            }


            expense.Description = dto.Description;
            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.CategoryId = dto.CategoryId;

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
