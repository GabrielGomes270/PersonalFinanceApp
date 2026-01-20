using PersonalFinanceApp.Domain.Entities;

namespace PersonalFinanceApp.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(int userId);
        Task<Category?> GetCategoryByIdAsync(int categoryId, int userId);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }
}
