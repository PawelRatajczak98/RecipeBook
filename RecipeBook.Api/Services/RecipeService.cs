using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Entities;
using RecipeBook.Api.Models;

namespace RecipeBook.Api.Services
{
    public interface IRecipeService
    {
        Task<Recipe> CreateRecipeAsync(Recipe recipe);
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(int id);
        Task<Recipe> UpdateRecipeAsync(int id, Recipe recipe);
    }

    public class RecipeService : IRecipeService
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public RecipeService(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {       
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;    
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes.ToListAsync();
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Recipe> UpdateRecipeAsync(int id, Recipe recipe)
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
    }
}
