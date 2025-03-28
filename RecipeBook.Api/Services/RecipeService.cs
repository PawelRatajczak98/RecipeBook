using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Data;
using RecipeBook.Api.Entities;
using RecipeBook.Api.Models;

namespace RecipeBook.Api.Services
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<Recipe> GetByIdAsync(int id);
        Task<Recipe> CreateAsync(RecipeCreateDto recipeCreateDto);
        Task<Recipe> UpdateAsync(int id, Recipe recipe);
        Task<bool> DeleteAsync(int id);
        Task<decimal> CalculateRecipeCostAsync(int recipeId);
        Task<List<Recipe>> GetRecipesWithingBudget();
        Task<List<Recipe>> GetRecipesUserCanPrepareAsync();
    }

    public class RecipeService : IRecipeService
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserContextService _userContextService;

        public RecipeService(AppDbContext context, IAuthService authService, IUserContextService userContextService)
        {
            _context = context;
            _authService = authService;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();
        }

        public async Task<Recipe> GetByIdAsync(int id)
        {
            return await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Recipe> CreateAsync(RecipeCreateDto dto)
        {
            if (dto == null)
            {
                throw new Exception("Empty Recipe");
            }
            
            var recipeIngredients = new List<RecipeIngredient>();
            
            foreach (var item in dto.RecipeIngredients)
            {
                var ingredient = await _context.Ingredients.FindAsync(item.IngredientId);
                if (ingredient == null)
                {
                    throw new Exception("Ingredient not found");
                }

                var recipeIngredient = new RecipeIngredient
                {
                    IngredientId = item.IngredientId,
                    Quantity = item.Quantity,
                    Unit = item.Unit
                };
                recipeIngredients.Add(recipeIngredient);
            }
            var recipe = new Recipe
            {
                Name = dto.Name,
                Description = dto.Description,
                RecipeIngredients = recipeIngredients
            };

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            return recipe;
        }


        public async Task<Recipe> UpdateAsync(int id, Recipe recipe)
        {
            var existingRecipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(r => r.Id ==id);
            
            if(existingRecipe == null)
            {
                throw new Exception("Not found");
            }
            
            existingRecipe.Name = recipe.Name;
            existingRecipe.Description = recipe.Description;
            existingRecipe.RecipeIngredients = recipe.RecipeIngredients;
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return false;

            _context.Recipes.Remove(recipe);
            return await _context.SaveChangesAsync() > 0;
        }

        
        public async Task<decimal> CalculateRecipeCostAsync(int recipeId)
        {
            var totalCost = await _context.RecipeIngredients
                .Include(ri => ri.Ingredient)
                .Where(ri => ri.RecipeId == recipeId)
                .SumAsync(ri => ri.Ingredient.PriceFor100Grams * (decimal)ri.Quantity);
            return totalCost;
        }

        public async Task<List<Recipe>> GetRecipesWithingBudget()
        {
            var userId = _userContextService.GetUserId();
            var user = await _context.Users.FindAsync(userId);
            
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (user.Budget == null)
                {
                    throw new Exception("User budget not set");
                }

            var recipesWithinBudget = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .Where(r => r.RecipeIngredients
                .Sum(ri => ri.Ingredient.PriceFor100Grams * (decimal)ri.Quantity) <= user.Budget)
                .ToListAsync();
            return recipesWithinBudget;
        }

        public async Task<List<Recipe>> GetRecipesUserCanPrepareAsync()
        {
            var userId = _userContextService.GetUserId();
            return await _context.Recipes
                .Where(r => r.RecipeIngredients
                .All(ri => _context.UserIngredients
                .Any(ui => ui.UserId == userId
                && ui.IngredientId == ri.IngredientId
                && ui.Quantity >= (decimal)ri.Quantity)))
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();
        }
    }
}