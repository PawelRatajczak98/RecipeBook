using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Entities;

namespace RecipeBook.Api.Controllers
{
    public class AdminController: ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<AppUser> _userManager;
        public AdminController(IAdminService adminService, UserManager<AppUser> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _adminService.GetUsersWithRolesAsync();
            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            var result = await _adminService.EditRolesAsync(username, roles);
            return Ok(result);
        }
    }
}
