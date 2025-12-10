namespace RecipeCraftApi.Application.DTOs
{
    public class IngredientTypeDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class IngredientTypeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class IngredientDto
    {
        public string Name { get; set; } = null!;
        public int IngredientTypeId { get; set; }
    }

    public class IngredientResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int IngredientTypeId { get; set; }
        public string IngredientTypeName { get; set; } = null!;
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = null!;
    }
}
