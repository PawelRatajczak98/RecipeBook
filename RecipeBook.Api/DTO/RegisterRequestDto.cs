using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Api.Models
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(16,MinimumLength = 4)]
        public string Password { get; set; } = string.Empty;

    }
}
