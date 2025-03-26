using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Data;
using RecipeBook.Api.Entities;

namespace RecipeBook.Api.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient> GetByIdAsync(int id);
        Task<Ingredient> CreateAsync(Ingredient ingredient);
        Task<Ingredient> UpdateAsync(int id, Ingredient ingredient);
        Task<bool> DeleteAsync(int id);
    }
    public class IngredientService : IIngredientService
    {
        private readonly AppDbContext _context;

        public IngredientService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient> GetByIdAsync(int id)
        {
            return await _context.Ingredients.FindAsync(id);
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingredient)
        {
            if (ingredient == null) 
            {
                throw new Exception("Empty Ingredient");
            }
            if (_context.Ingredients.Any( i => i.Name == ingredient.Name))
            {
                throw new Exception("Item already in db");
            }
               
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task<Ingredient> UpdateAsync(int id, Ingredient ingredient)
        {
            var entity = await _context.Ingredients.FindAsync(id);
            if (entity == null) return null;
            entity.Description = ingredient.Description;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Ingredients.FindAsync(id);
            if (entity == null) return false;
            _context.Ingredients.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
