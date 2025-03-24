using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Entities;
using RecipeBook.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly RecipeService _recipeService;
        public RecipesController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Recipe recipe)
        {
            var createdRecipe = await _recipeService.CreateRecipeAsync(recipe);
            return Ok(createdRecipe);
        }

        // GET: api/<RecipesController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        // GET api/<RecipesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            return Ok(recipe);
        }

        // PUT api/<RecipesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Recipe updatedRecipe)
        {
            var recipe = await _recipeService.UpdateRecipeAsync(id, updatedRecipe);
            return Ok(recipe);
        }

       
    }
}
