using RecipeBook.Api.Entities;

namespace RecipeBook.Api.Models
{
    public class RecipeCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RecipeIngredientDto> RecipeIngredients { get; set; }
    }
}
