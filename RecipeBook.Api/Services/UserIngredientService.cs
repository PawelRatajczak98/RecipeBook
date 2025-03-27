using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Data;
using RecipeBook.Api.DTO;
using RecipeBook.Api.Entities;
using RecipeBook.Api.Models;

namespace RecipeBook.Api.Services
{
    public interface IUserIngredientService
    {
        Task<List<UserIngredient>> GetAllAsync();
        Task<UserIngredient> GetIngredientById(int id, string userId);
        Task<UserIngredient> UpdateAsync(UserIngredientUpdateDto updatedUserIngredient);
        Task<bool> DeleteAsync(int id, string userId);
        Task<UserIngredient> CreateAsync(UserIngredientCreateDto userIngredientCreateDto);
    }
    public class UserIngredientService : IUserIngredientService
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IUserContextService _userContextService;

        public UserIngredientService(AppDbContext context, IAuthService authService, IUserContextService userContextService)
        {
             _context = context;
            _authService = authService;
            _userContextService = userContextService;
        }

        public async Task<List<UserIngredient>> GetAllAsync()
        {
            var userId = _authService.GetCurrentUserId();
            return await _context.UserIngredients
                .Where(ui => ui.UserId == userId)
                .Include(ui => ui.Ingredient)
                .ToListAsync();
        }

        public async Task<UserIngredient> GetIngredientById(int ingredientId, string userId)
        {
            return await _context.UserIngredients
                .Include(ui => ui.Ingredient)
                .FirstOrDefaultAsync(ui => ui.UserId == userId && ui.IngredientId == ingredientId);
        }

        public async Task<UserIngredient> UpdateAsync(UserIngredientUpdateDto updatedUserIngredient)
        {
            
            var userId = _userContextService.GetUserId;

            var userIngredient = await _context.UserIngredients
                .Include(ui => ui.Ingredient)
                .FirstOrDefaultAsync(ui => ui.UserId.Equals(userId) 
                && ui.Ingredient.Name.Equals(updatedUserIngredient.Name));

            if (userIngredient == null)
                return null;
            userIngredient.Quantity = updatedUserIngredient.Quantity;
            await _context.SaveChangesAsync();
            return userIngredient;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var userIngredient = await _context.UserIngredients.FindAsync(id);
            if (userIngredient == null || userIngredient.UserId != userId)
                return false;
            _context.UserIngredients.Remove(userIngredient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserIngredient> CreateAsync(UserIngredientCreateDto userIngredientCreateDto)
        {
            var currentUserId = _userContextService.GetUserId();
            if (currentUserId == null)
            {
                throw new Exception("User not logged in.");
            }

            var ingredient = await _context.Ingredients.FirstOrDefaultAsync( i => i.Id == userIngredientCreateDto.IngredientId );
            if ( ingredient == null )
            {
                throw new Exception("Ingredient not exist");
            }
            
            var alreadyExist = await _context.UserIngredients
                .AnyAsync(ui=>ui.UserId == currentUserId && ui.IngredientId == ingredient.Id);
            if ( alreadyExist)
            {
                throw new Exception("You already have this product.");
            }

            var userIngredient = new UserIngredient
            {
                UserId = currentUserId,
                IngredientId = ingredient.Id,
                Ingredient = ingredient,
                Quantity = userIngredientCreateDto.Quantity
            };

            _context.UserIngredients.Add(userIngredient);
            await _context.SaveChangesAsync();           
            return userIngredient;
        }
    }
}
