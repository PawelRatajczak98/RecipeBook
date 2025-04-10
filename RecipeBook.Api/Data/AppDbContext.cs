﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Api.Entities;
using System.Reflection.Emit;
namespace RecipeBook.Api.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<UserIngredient> UserIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Budget)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Budget)
                .HasDefaultValue(0m);

            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            
            modelBuilder.Entity<Ingredient>()
                .Property(i => i.PriceFor100Grams)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

            modelBuilder.Entity<UserIngredient>()
                .HasKey(ui => new { ui.UserId, ui.IngredientId });
            
            modelBuilder.Entity<UserIngredient>()
                .Property(ui => ui.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<UserIngredient>()
                .HasOne(ui=>ui.User)
                .WithMany(u=>u.UserIngredients)
                .HasForeignKey(ui => ui.UserId);

            modelBuilder.Entity<UserIngredient>()
                .HasOne(ui => ui.Ingredient)
                .WithMany(i=>i.UserIngredients)
                .HasForeignKey(ui => ui.IngredientId);

        }

    }
}
