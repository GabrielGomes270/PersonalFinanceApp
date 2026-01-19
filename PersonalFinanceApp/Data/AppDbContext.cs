using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Domain.Entities;

namespace PersonalFinanceApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        DbSet<User> Users { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Expense> Expenses { get; set; }
    }
}
