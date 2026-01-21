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
    public class GetAnnualSummaryDtoController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public GetAnnualSummaryDtoController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        [HttpGet("summary/annual")]
        public async Task<IActionResult> GetAnnualSummary(int year)
        {
            var userId = User.GetUserId();

            var summary = await _expenseRepository.GetAnnualSummaryAsync(userId, year);

            return Ok(summary);
        }
    }
}
