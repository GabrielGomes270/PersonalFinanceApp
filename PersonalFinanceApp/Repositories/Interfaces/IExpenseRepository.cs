using PersonalFinanceApp.Domain.Entities;
using PersonalFinanceApp.DTOs;

namespace PersonalFinanceApp.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<PagedResultDto<Expense>> GetAllExpensesAsync(
            int userID,
            int? categoryId,
            int? month,
            int page,
            int pageSize,
            string orderBy,
            string direction);

        Task<Expense?> GetExpenseByIdAsync(int id, int userId);
        Task AddExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(Expense expense);

        Task<MonthlySummaryDto> GetMonthlySummaryAsync(
        int userId,
        int year,
        int month);

        Task<AnnualSummaryDto> GetAnnualSummaryAsync(int userId,  int year);
    }
}
