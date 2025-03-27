namespace RecipeBook.Api.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceFor100Grams { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<UserIngredient> UserIngredients { get; set; } = new List<UserIngredient>();
    }
}
