using RecipeCraftApi.Domain.Entities;
using RecipeCraftApi.Domain.Interfaces;
using RecipeCraftApi.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeCraftApi.Application.Services
{
    public class IngredientTypeService
    {
        private readonly IIngredientTypeRepository _repo;
        public IngredientTypeService(IIngredientTypeRepository repo) => _repo = repo;

        public async Task<IEnumerable<IngredientTypeResponseDto>> GetAll()
        {
            var types = await _repo.GetAllAsync();
            var result = new List<IngredientTypeResponseDto>();
            foreach (var t in types)
                result.Add(new IngredientTypeResponseDto { Id = t.Id, Name = t.Name, Description = t.Description });
            return result;
        }

        public async Task<IngredientTypeResponseDto> GetById(int id)
        {
            var type = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("IngredientType not found");
            return new IngredientTypeResponseDto { Id = type.Id, Name = type.Name, Description = type.Description };
        }

        public async Task<IngredientTypeResponseDto> Create(IngredientTypeDto dto)
        {
            var existing = await _repo.GetByNameAsync(dto.Name);
            if (existing != null) throw new Exception("IngredientType with this name already exists");

            var type = new IngredientType { Name = dto.Name, Description = dto.Description };
            await _repo.AddAsync(type);

            return new IngredientTypeResponseDto { Id = type.Id, Name = type.Name, Description = type.Description };
        }

        public async Task<IngredientTypeResponseDto> Update(int id, IngredientTypeDto dto)
        {
            var type = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("IngredientType not found");
            type.Name = dto.Name;
            type.Description = dto.Description;
            await _repo.UpdateAsync(type);
            return new IngredientTypeResponseDto { Id = type.Id, Name = type.Name, Description = type.Description };
        }

        public async Task Delete(int id)
        {
            var type = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("IngredientType not found");
            await _repo.DeleteAsync(type);
        }
    }
}
