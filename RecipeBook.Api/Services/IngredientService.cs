using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Entities;

namespace RecipeBook.Api.Services
{
    public interface IIngredientService
    {
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient);
        Task<Ingredient> UpdateIngredientAsync(int id, Ingredient ingredient);
        Task<bool> DeleteIngredientAsync(int id);
    }
    public class IngredientService : IIngredientService
    {
        private readonly AppDbContext _appDbContext;

        public IngredientService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            return await _appDbContext.Ingredients.ToListAsync();
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient)
        {
            if (ingredient == null) 
            {
                throw new Exception("Empty Ingredient");
            }
            if (_appDbContext.Ingredients.Any( i => i.Name == ingredient.Name))
            {
                throw new Exception("Item already in db");
            }
               
            _appDbContext.Ingredients.Add(ingredient);
            _appDbContext.SaveChangesAsync();
            return ingredient;
        }

        public async Task<Ingredient> UpdateIngredientAsync(int id, Ingredient ingredient)
        {
            var entity = await _appDbContext.Ingredients.FindAsync(id);
            if (entity == null) return null;
            entity.Description = ingredient.Description;
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteIngredientAsync(int id)
        {
            var entity = await _appDbContext.Ingredients.FindAsync(id);
            if (entity == null) return false;
            _appDbContext.Ingredients.Remove(entity);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

    }
}
