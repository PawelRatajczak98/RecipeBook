using RecipeBook.Api.Entities;
namespace RecipeBook.Api.Services
{
    public interface IRecipeService
    {
        Task<Recipe> CreateRecipeAsync(Recipe recipe);
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<Recipe>GetRecipeIdAsync(int id);
        Task<Recipe> UpdateRecipeAsync(Recipe recipe);
    }
}
