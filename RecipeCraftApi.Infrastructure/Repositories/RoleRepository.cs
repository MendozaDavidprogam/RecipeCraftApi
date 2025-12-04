using Microsoft.EntityFrameworkCore;
using RecipeCraftApi.Domain.Entities;
using RecipeCraftApi.Domain.Interfaces;
using RecipeCraftApi.Infrastructure.Database.Context;
using System.Threading.Tasks;

namespace RecipeCraftApi.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) => _context = context;

        public async Task<Role> GetByIdAsync(int id) => await _context.Roles.FindAsync(id);

        public async Task<Role> GetByNameAsync(string name) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

        // Métodos para agregar/actualizar roles si fuese necesario
        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
    }
}
