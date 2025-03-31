using RecipeBook.Api.Data;

namespace RecipeBook.Api.Services
{
    public interface IBudgetService
    {
        Task<bool> IncreaseBudgetAsync(decimal amount);
        Task<bool> DecreaseBudgetAsync(decimal amount);
        Task<decimal?> GetBudgetAsync();
    }
    public class BudgetService : IBudgetService
    {
        private readonly AppDbContext _context;
        private readonly IUserContextService _userContextService;
        public BudgetService(AppDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _userContextService = userContextService;
        }
        public async Task<bool> IncreaseBudgetAsync(decimal amount)
        {
            var user = await _context.Users.FindAsync(_userContextService.GetUserId());
            if (user == null)
            {
                return false;
            }
            user.Budget += amount;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DecreaseBudgetAsync(decimal amount)
        {
            var user = await _context.Users.FindAsync(_userContextService.GetUserId());
            if (user == null)
            {
                return false;
            }
            if(user.Budget < amount)
            {
                return false;
            }
            user.Budget -= amount;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<decimal?> GetBudgetAsync()
        {
            var user = await _context.Users.FindAsync(_userContextService.GetUserId());
            return user.Budget;
        }

        
    }
}
