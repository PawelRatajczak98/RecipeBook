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

        public async Task<Recipe> CreateAsync(RecipeCreateDto model)
        {
            var recipe = new Recipe
            {
                Name = model.Name,
                Description = model.Description,
                RecipeIngredients = model.RecipeIngredients.Select(item => new RecipeIngredient
                {
                    IngredientId = item.IngredientId,
                    Quantity = item.Quantity,
                    Unit = item.Unit
                }).ToList()
            };

            _context.Recipes.Add(recipe);
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
    }
}
