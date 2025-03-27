using Microsoft.AspNetCore.Identity;

namespace RecipeBook.Api.Entities
{
    public class AppUser : IdentityUser
    {
        public decimal? Budget {  get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; } = [];

    }
}
