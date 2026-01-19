using PersonalFinanceApp.Domain.Entities;

namespace PersonalFinanceApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
    }
}
