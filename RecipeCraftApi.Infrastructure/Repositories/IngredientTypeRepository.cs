using Microsoft.EntityFrameworkCore;
using RecipeCraftApi.Domain.Entities;
using RecipeCraftApi.Domain.Interfaces;
using RecipeCraftApi.Infrastructure.Database.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeCraftApi.Infrastructure.Repositories
{
    public class IngredientTypeRepository : IIngredientTypeRepository
    {
        private readonly AppDbContext _context;
        public IngredientTypeRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(IngredientType type)
        {
            await _context.Set<IngredientType>().AddAsync(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IngredientType type)
        {
            _context.Set<IngredientType>().Remove(type);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<IngredientType>> GetAllAsync() =>
            await _context.Set<IngredientType>().ToListAsync();

        public async Task<IngredientType> GetByIdAsync(int id) =>
            await _context.Set<IngredientType>().FindAsync(id);

        public async Task<IngredientType> GetByNameAsync(string name) =>
            await _context.Set<IngredientType>().FirstOrDefaultAsync(t => t.Name == name);

        public async Task UpdateAsync(IngredientType type)
        {
            _context.Set<IngredientType>().Update(type);
            await _context.SaveChangesAsync();
        }
    }
}
