using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeBook.Api.Migrations
{
    /// <inheritdoc />
    public partial class PriceAddedToIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceFor100Grams",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceFor100Grams",
                table: "Ingredients");
        }
    }
}
