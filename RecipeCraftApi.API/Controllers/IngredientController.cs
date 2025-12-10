using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeCraftApi.Application.DTOs;
using RecipeCraftApi.Application.Services;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeCraftApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IngredientController : ControllerBase
    {
        private readonly IngredientService _service;
        public IngredientController(IngredientService service) => _service = service;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role);

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetById(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IngredientDto dto)
        {
            var result = await _service.Create(dto, CurrentUserId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] IngredientDto dto)
        {
            var result = await _service.Update(id, dto, CurrentUserId, CurrentUserRole);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id, CurrentUserId, CurrentUserRole);
            return NoContent();
        }
    }
}
