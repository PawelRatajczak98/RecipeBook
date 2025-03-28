using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet("budget")]
        public async Task<IActionResult> GetBudget()
        {
            var budget = await _budgetService.GetBudgetAsync();
            return Ok(budget);
        }

        [HttpPost("increase")]
        public async Task<IActionResult> IncreaseBudget(decimal amount)
        {
            var budget = await _budgetService.IncreaseBudgetAsync(amount);
            return Ok(budget);
        }

        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseBudget(decimal amount)
        {
            var budget = await _budgetService.DecreaseBudgetAsync(amount);
            return Ok(budget);
        }
       
    }
}
