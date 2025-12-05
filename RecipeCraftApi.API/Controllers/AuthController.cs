using Microsoft.AspNetCore.Mvc;
using RecipeCraftApi.Application.DTOs;
using RecipeCraftApi.Application.Services;
using System.Threading.Tasks;

namespace RecipeCraftApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService) => _userService = userService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.Register(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var token = await _userService.Login(dto);
            return Ok(new { Token = token });
        }
    }
}
