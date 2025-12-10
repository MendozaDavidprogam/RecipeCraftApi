using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeCraftApi.Application.DTOs;
using RecipeCraftApi.Application.Services;
using System.Threading.Tasks;

namespace RecipeCraftApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class IngredientTypeController : ControllerBase
    {
        private readonly IngredientTypeService _service;
        public IngredientTypeController(IngredientTypeService service) => _service = service;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IngredientTypeDto dto) => Ok(await _service.Create(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] IngredientTypeDto dto) => Ok(await _service.Update(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
