namespace RecipeCraftApi.Domain.Entities
{
    public class Ingredient
    {
        public int Id { get; set; } // PK
        public string Name { get; set; } = null!;

        public int IngredientTypeId { get; set; } // FK → IngredientType
        public IngredientType? IngredientType { get; set; }

        public int CreatedByUserId { get; set; } // FK → User
        public User? CreatedByUser { get; set; }
    }
}
