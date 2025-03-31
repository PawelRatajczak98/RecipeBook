using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.DTO;
using RecipeBook.Api.Entities;

namespace RecipeBook.Api.Controllers
{
    public interface IAdminService
    {
        Task<List<UserDto>> GetUsersWithRolesAsync();
        Task<string> EditRolesAsync(string username, string roles);
    }
    public class AdminService(UserManager<AppUser> userManager) : IAdminService
    {
        public async Task<List<UserDto>> GetUsersWithRolesAsync()
        {
            var users = await userManager.Users
                .OrderBy(x => x.UserName)
                .SelectMany(x => x.UserRoles.Select(r => new UserDto
                {
                    UserName = x.UserName,
                    RoleName = r.Role.Name
                }))
                .ToListAsync();

            return users;
        }

        public async Task<string> EditRolesAsync(string username, string roles)
        {
            if (string.IsNullOrEmpty(roles))
                return "You must select at least one role";

            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(username);

            if (user == null)
                return "User not found";

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return "Failed to add to roles";

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return "Failed to remove from roles";

            return $"Successfully changed {username} role to {roles}";
        }
    }
}
