using RecipeCraftApi.Domain.Entities;
using RecipeCraftApi.Domain.Interfaces;
using RecipeCraftApi.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeCraftApi.Application.Services
{
    public class IngredientService
    {
        private readonly IIngredientRepository _repo;
        private readonly IIngredientTypeRepository _typeRepo;

        public IngredientService(IIngredientRepository repo, IIngredientTypeRepository typeRepo)
        {
            _repo = repo;
            _typeRepo = typeRepo;
        }

        public async Task<IEnumerable<IngredientResponseDto>> GetAll() 
        {
            var ingredients = await _repo.GetAllAsync();
            var result = new List<IngredientResponseDto>();
            foreach (var i in ingredients)
                result.Add(new IngredientResponseDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    IngredientTypeId = i.IngredientTypeId,
                    IngredientTypeName = i.IngredientType?.Name ?? "",
                    CreatedByUserId = i.CreatedByUserId,
                    CreatedByUserName = i.CreatedByUser?.Name ?? ""
                });
            return result;
        }

        public async Task<IngredientResponseDto> GetById(int id)
        {
            var i = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Ingredient not found");
            return new IngredientResponseDto
            {
                Id = i.Id,
                Name = i.Name,
                IngredientTypeId = i.IngredientTypeId,
                IngredientTypeName = i.IngredientType?.Name ?? "",
                CreatedByUserId = i.CreatedByUserId,
                CreatedByUserName = i.CreatedByUser?.Name ?? ""
            };
        }

        public async Task<IngredientResponseDto> Create(IngredientDto dto, int userId)
        {
            var type = await _typeRepo.GetByIdAsync(dto.IngredientTypeId) ?? throw new KeyNotFoundException("IngredientType not found");
            var existing = await _repo.GetByNameAsync(dto.Name);
            if (existing != null) throw new Exception("Ingredient with this name already exists");

            var ingredient = new Ingredient { Name = dto.Name, IngredientTypeId = dto.IngredientTypeId, CreatedByUserId = userId };
            await _repo.AddAsync(ingredient);

            return new IngredientResponseDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                IngredientTypeId = ingredient.IngredientTypeId,
                IngredientTypeName = type.Name,
                CreatedByUserId = userId,
                CreatedByUserName = ingredient.CreatedByUser?.Name ?? ""
            };
        }

        public async Task<IngredientResponseDto> Update(int id, IngredientDto dto, int userId, string role)
        {
            var ingredient = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Ingredient not found");
            if (role != "Admin" && ingredient.CreatedByUserId != userId) throw new UnauthorizedAccessException();

            var type = await _typeRepo.GetByIdAsync(dto.IngredientTypeId) ?? throw new KeyNotFoundException("IngredientType not found");
            ingredient.Name = dto.Name;
            ingredient.IngredientTypeId = dto.IngredientTypeId;

            await _repo.UpdateAsync(ingredient);

            return new IngredientResponseDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                IngredientTypeId = ingredient.IngredientTypeId,
                IngredientTypeName = type.Name,
                CreatedByUserId = ingredient.CreatedByUserId,
                CreatedByUserName = ingredient.CreatedByUser?.Name ?? ""
            };
        }

        public async Task Delete(int id, int userId, string role)
        {
            var ingredient = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Ingredient not found");
            if (role != "Admin" && ingredient.CreatedByUserId != userId) throw new UnauthorizedAccessException();
            await _repo.DeleteAsync(ingredient);
        }
    }
}
