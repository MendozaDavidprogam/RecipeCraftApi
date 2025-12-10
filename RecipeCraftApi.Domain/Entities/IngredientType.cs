using System.Collections.Generic;

namespace RecipeCraftApi.Domain.Entities
{
    public class IngredientType
    {
        public int Id { get; set; } // PK
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<Ingredient>? Ingredients { get; set; }
    }
}
