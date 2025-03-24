using System.Text.Json.Serialization;

namespace RecipeBook.Api.Entities
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        [JsonIgnore]
        public Recipe Recipe { get; set; }
        public int IngredientId { get; set;}
        public Ingredient Ingredient { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
    }
}
