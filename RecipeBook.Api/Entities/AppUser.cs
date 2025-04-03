﻿using Microsoft.AspNetCore.Identity;

namespace RecipeBook.Api.Entities
{
    public class AppUser : IdentityUser
    {
        public decimal? Budget {  get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
        public ICollection<UserIngredient> UserIngredients { get; set; } = new List<UserIngredient>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
