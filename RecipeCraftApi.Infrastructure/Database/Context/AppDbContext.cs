using Microsoft.EntityFrameworkCore;
using RecipeCraftApi.Domain.Entities;

namespace RecipeCraftApi.Infrastructure.Database.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<IngredientType> IngredientTypes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            // Configuración de unicidad para IngredientType.Name
            modelBuilder.Entity<IngredientType>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // Configuración de unicidad para Ingredient.Name
            modelBuilder.Entity<Ingredient>()
                .HasIndex(i => i.Name)
                .IsUnique();

            // Relación Ingredient → IngredientType
            modelBuilder.Entity<Ingredient>()
                .HasOne(i => i.IngredientType)
                .WithMany(t => t.Ingredients)
                .HasForeignKey(i => i.IngredientTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Ingredient → User (creador)
            modelBuilder.Entity<Ingredient>()
                .HasOne(i => i.CreatedByUser)
                .WithMany()
                .HasForeignKey(i => i.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
