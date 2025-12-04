using RecipeCraftApi.Domain.Entities;
using System.Threading.Tasks;

namespace RecipeCraftApi.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string name);
    }
}