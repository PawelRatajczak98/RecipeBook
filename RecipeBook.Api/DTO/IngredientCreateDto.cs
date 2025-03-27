namespace RecipeBook.Api.DTO
{
    public class IngredientCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceFor100Grams { get; set; }
    }
}
