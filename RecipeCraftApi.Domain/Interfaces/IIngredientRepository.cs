using RecipeCraftApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeCraftApi.Domain.Interfaces
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient> GetByIdAsync(int id);
        Task<Ingredient> GetByNameAsync(string name);
        Task<IEnumerable<Ingredient>> GetByUserIdAsync(int userId);
        Task AddAsync(Ingredient ingredient);
        Task UpdateAsync(Ingredient ingredient);
        Task DeleteAsync(Ingredient ingredient);
    }
}
