using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Entities;
using RecipeBook.Api.Models;
using RecipeBook.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(RecipeCreateDto recipe)
        {
            var createdRecipe = await _recipeService.CreateAsync(recipe);
            return Ok(createdRecipe);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var recipes = await _recipeService.GetAllAsync();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            return Ok(recipe);
        }

        [HttpGet("{recipeId}/cost")]
        public async Task<IActionResult> GetRecipeCost(int recipeId)
        {
            var cost = await _recipeService.CalculateRecipeCostAsync(recipeId);
            return Ok(cost);
        }

        [HttpGet("within-budget")]
        [Authorize]
        public async Task<IActionResult> GetRecipesWithinBudget()
        {
            var recipes = await _recipeService.GetRecipesWithingBudget();
            return Ok(recipes);
        }

        [HttpGet("can-prepare")]
        [Authorize]
        public async Task<IActionResult> GetRecipesUserCanPrepare()
        {
            var recipes = await _recipeService.GetRecipesUserCanPrepareAsync();
            return Ok(recipes);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Put(int id, Recipe updatedRecipe)
        {
            var recipe = await _recipeService.UpdateAsync(id, updatedRecipe);
            return Ok(recipe);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _recipeService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

    }
}
