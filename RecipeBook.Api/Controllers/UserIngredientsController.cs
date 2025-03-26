using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Services;
using RecipeBook.Api.Entities;
using System.Security.Claims;
using RecipeBook.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace RecipeBook.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserIngredientsController : ControllerBase
    {
        private readonly IUserIngredientService _userIngredientService;

        public UserIngredientsController(IUserIngredientService userIngredientService)
        {
            _userIngredientService = userIngredientService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userIngredientService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserIngredientCreateDto userIngredientCreateDto)
        {
            var result = await _userIngredientService.CreateAsync(userIngredientCreateDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserIngredient userIngredient)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _userIngredientService.UpdateAsync(id, userIngredient, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _userIngredientService.DeleteAsync(id, userId);
            return success ? NoContent() : NotFound();
        }
    }
}
