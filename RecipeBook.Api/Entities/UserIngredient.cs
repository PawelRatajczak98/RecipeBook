using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Api.Entities
{
    public class UserIngredient
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        
        public int IngredientId { get; set; }

        public Ingredient Ingredient { get; set; }

        public decimal Quantity { get; set; }
    }
}
