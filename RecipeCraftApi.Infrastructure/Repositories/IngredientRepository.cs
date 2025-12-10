using Microsoft.EntityFrameworkCore;
using RecipeCraftApi.Domain.Entities;
using RecipeCraftApi.Domain.Interfaces;
using RecipeCraftApi.Infrastructure.Database.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeCraftApi.Infrastructure.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly AppDbContext _context;
        public IngredientRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Ingredient ingredient)
        {
            await _context.Set<Ingredient>().AddAsync(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ingredient ingredient)
        {
            _context.Set<Ingredient>().Remove(ingredient);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync() =>
            await _context.Set<Ingredient>()
                .Include(i => i.IngredientType)
                .Include(i => i.CreatedByUser)
                .ToListAsync();

        public async Task<Ingredient> GetByIdAsync(int id) =>
            await _context.Set<Ingredient>()
                .Include(i => i.IngredientType)
                .Include(i => i.CreatedByUser)
                .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<Ingredient> GetByNameAsync(string name) =>
            await _context.Set<Ingredient>()
                .Include(i => i.IngredientType)
                .Include(i => i.CreatedByUser)
                .FirstOrDefaultAsync(i => i.Name == name);

        public async Task<IEnumerable<Ingredient>> GetByUserIdAsync(int userId) =>
            await _context.Set<Ingredient>()
                .Where(i => i.CreatedByUserId == userId)
                .Include(i => i.IngredientType)
                .ToListAsync();

        public async Task UpdateAsync(Ingredient ingredient)
        {
            _context.Set<Ingredient>().Update(ingredient);
            await _context.SaveChangesAsync();
        }
    }
}
