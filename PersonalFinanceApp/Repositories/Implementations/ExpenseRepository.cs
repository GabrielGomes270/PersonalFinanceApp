using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Domain.Entities;
using PersonalFinanceApp.DTOs;
using PersonalFinanceApp.Repositories.Interfaces;
using System.Linq.Expressions;

namespace PersonalFinanceApp.Repositories.Implementations
{
    public class ExpenseRepository : IExpenseRepository
    {

        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDto<Expense>> GetAllExpensesAsync(
            int userID, 
            int? categoryId, 
            int? month, 
            int page, 
            int pageSize,
            string orderBy,
            string direction)
        {
            var query = _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userID)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(e => e.CategoryId == categoryId);
            }

            if (month.HasValue)
            {
                query = query.Where(e => e.Date.Month == month);
            }

            query = ApplyOrdering(query, orderBy, direction);

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<Expense>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id, int userId)
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            _context .Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }




        private static IQueryable<Expense> ApplyOrdering(
        IQueryable<Expense> query,
        string orderBy,
        string direction)
        {
            var isDesc = direction?.ToLower() == "desc";

            return orderBy?.ToLower() switch
            {
                "date" => isDesc
                    ? query.OrderByDescending(e => e.Date)
                    : query.OrderBy(e => e.Date),

                "amount" => isDesc
                    ? query.OrderByDescending(e => e.Amount)
                    : query.OrderBy(e => e.Amount),

                "description" => isDesc
                    ? query.OrderByDescending(e => e.Description)
                    : query.OrderBy(e => e.Description),

                "category" => isDesc
                    ? query.OrderByDescending(e => e.Category.Name)
                    : query.OrderBy(e => e.Category.Name),

                _ => query.OrderByDescending(e => e.Date)
            };
        }

        public async Task<MonthlySummaryDto> GetMonthlySummaryAsync(int userId,  int year, int month)
        {
            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Where(e =>
                    e.UserId == userId &&
                    e.Date.Year == year &&
                    e.Date.Month == month)
                .ToListAsync();

            return new MonthlySummaryDto
            {
                Year = year,
                Month = month,
                TotalAmount = expenses.Sum(e => e.Amount),
                ByCategory = expenses
                .GroupBy(e => new
                {
                    e.CategoryId,
                    e.Category.Name
                })
                .Select(g => new CategorySummaryDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.Name,
                    Total = g.Sum(e => e.Amount)
                })
                .ToList()
            };
        }

        public async Task<AnnualSummaryDto> GetAnnualSummaryAsync(int userId, int year)
        {
            var expenses = await _context.Expenses
                .Where(e =>
                    e.UserId == userId &&
                    e.Date.Year == year)
                .ToListAsync();

            return new AnnualSummaryDto
            {
                Year = year,
                TotalAmount = expenses.Sum(e => e.Amount),
                ByMonth = expenses
                    .GroupBy(e => e.Date.Month)
                    .Select(g => new MonthlyTotalDto
                    {
                        Month = g.Key,
                        Total = g.Sum(e => e.Amount)
                    })
                    .OrderBy(m => m.Month)
                    .ToList()
            };
        }
    }
}


