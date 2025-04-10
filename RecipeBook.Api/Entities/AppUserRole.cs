﻿using Microsoft.AspNetCore.Identity;

namespace RecipeBook.Api.Entities
{
    public class AppUserRole:IdentityUserRole<string>
    {
        public AppUser User { get; set; } = null!;
        public AppRole Role { get; set; } = null!;
    }
}
