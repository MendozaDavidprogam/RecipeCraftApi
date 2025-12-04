using Microsoft.AspNetCore.Mvc;
using RecipeCraftApi.Application.DTOs;
using RecipeCraftApi.Application.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RecipeCraftApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService) => _userService = userService;

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role);

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() => Ok(await _userService.GetAllUsers());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (CurrentUserRole != "Admin" && CurrentUserId != id) return Forbid();
            return Ok(await _userService.GetUserById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            if (CurrentUserRole != "Admin" && CurrentUserId != id) return Forbid();
            return Ok(await _userService.UpdateUser(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (CurrentUserRole != "Admin" && CurrentUserId != id) return Forbid();
            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}