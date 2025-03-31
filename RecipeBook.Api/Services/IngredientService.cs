using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Data;
using RecipeBook.Api.DTO;
using RecipeBook.Api.Entities;

namespace RecipeBook.Api.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient> GetByIdAsync(int id);
        Task<Ingredient> CreateAsync(IngredientCreateDto dto);
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

        public async Task<Ingredient> CreateAsync(IngredientCreateDto dto)
        {
            if (dto == null)
            {
                throw new Exception("Empty Ingredient");
            }
            if (_context.Ingredients.Any(i => i.Name == dto.Name))
            {
                throw new Exception("Item already created in database");
            }
            var ingredient = new Ingredient
            {
                Name = dto.Name,
                Description = dto.Description,
                PriceFor100Grams = dto.PriceFor100Grams
            };
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task<Ingredient> UpdateAsync(int id, Ingredient updatedIngredient)
        {
            var existingIngredient = await _context.Ingredients.FindAsync(id);
            if (existingIngredient == null) return null;
            existingIngredient.Description = updatedIngredient.Description;
            existingIngredient.PriceFor100Grams = updatedIngredient.PriceFor100Grams;
            await _context.SaveChangesAsync();
            return existingIngredient;
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
