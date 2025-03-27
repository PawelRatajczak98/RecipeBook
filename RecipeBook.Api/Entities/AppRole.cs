using Microsoft.AspNetCore.Identity;

namespace RecipeBook.Api.Entities
{
    public class AppRole : IdentityRole
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
    }
}
