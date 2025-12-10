using RecipeCraftApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeCraftApi.Domain.Interfaces
{
    public interface IIngredientTypeRepository
    {
        Task<IEnumerable<IngredientType>> GetAllAsync();
        Task<IngredientType> GetByIdAsync(int id);
        Task<IngredientType> GetByNameAsync(string name);
        Task AddAsync(IngredientType type);
        Task UpdateAsync(IngredientType type);
        Task DeleteAsync(IngredientType type);
    }
}
