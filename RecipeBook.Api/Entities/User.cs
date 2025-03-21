using Microsoft.AspNetCore.Identity;

namespace RecipeBook.Api.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nationality { get; set; }
    }
}
