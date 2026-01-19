using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Domain.Entities;
using PersonalFinanceApp.Repositories.Interfaces;

namespace PersonalFinanceApp.Repositories.Implementations
{
    public class ExpenseRepository : IExpenseRepository
    {

        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync(
            int userID, 
            int? categoryId, 
            int? month, 
            int page, 
            int pageSize)
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

            return await query
                .OrderByDescending(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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
    }
}
