using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Extensions;
using PersonalFinanceApp.Repositories.Interfaces;

namespace PersonalFinanceApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GetMonthlySummaryController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public GetMonthlySummaryController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        [HttpGet("summary/monthly")]
        public async Task<IActionResult> GetMonthlySummary(int year, int month)
        {
            var userId = User.GetUserId();

            var summary = await _expenseRepository.GetMonthlySummaryAsync(userId, year, month);

            return Ok(summary);
        }
    }
}
