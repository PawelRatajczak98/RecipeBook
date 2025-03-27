namespace RecipeBook.Api.Models
{
    public class RecipeIngredientDto
    {
        public int IngredientId { get; set; }
        public double Quantity { get; set; }
        public string MeasurementUnit { get; set; }
    }
}
