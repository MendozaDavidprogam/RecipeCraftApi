using Microsoft.EntityFrameworkCore;
using RecipeCraftApi.Domain.Entities;
using RecipeCraftApi.Domain.Interfaces;
using RecipeCraftApi.Infrastructure.Database.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeCraftApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); // Persistir en BD
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(); // Persistir en BD
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.Include(u => u.Role).ToListAsync();

        public async Task<User> GetByEmailAsync(string email) =>
            await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> GetByIdAsync(int id) =>
            await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(); // Persistir en BD
        }
    }
}
