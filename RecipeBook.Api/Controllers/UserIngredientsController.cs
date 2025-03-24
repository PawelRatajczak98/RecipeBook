using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Api.Services;
using RecipeBook.Api.Entities;
using System.Security.Claims;
using RecipeBook.Api.Models;

namespace RecipeBook.Api.Controllers
{
    public class UserIngredientsController : ControllerBase
    {
        private readonly IUserIngredientService _userIngredientService;
        private readonly IAuthService _authService;

        public UserIngredientsController(IUserIngredientService userIngredientService, IAuthService authService)
        {
            _userIngredientService = userIngredientService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = _authService.GetCurrentUserId();
            var userIngredients = await _userIngredientService.GetUserIngredientsAsync(userId);
            return Ok(userIngredients);
        }

        [HttpPost]

        public async Task<IActionResult> Post(UserIngredientCreateDto userIngredientCreateDto)
        {
            var userIngredient = await _userIngredientService.CreateUserIngredientAsync(userIngredientCreateDto);
            return Ok(userIngredient);
        }
    }
}
