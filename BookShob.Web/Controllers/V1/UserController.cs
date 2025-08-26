using BookShob.Application.Interfaces;
using BookShob.Domain.Entities;
using BookShob.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShob.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtService _jwtService;

        public UserController(IUserRepository userRepo, JwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userRepo.CreateUserAsync(user, dto.Password);

            if (result.Succeeded)
                return Ok("User created successfully");

            // 👇 Return real error messages
            return BadRequest(result.Errors.Select(e => e.Description));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.Username);
            if (user == null) return Unauthorized("Invalid credentials");

            // check password via UserManager inside repo
            // (let’s add this method in IUserRepository → CheckPasswordAsync)

            var valid = await _userRepo.CheckPasswordAsync(user, dto.Password);
            if (!valid) return Unauthorized("Invalid credentials");

            var roles = await _userRepo.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new { Token = token });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.Identity?.Name;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok(new { Id = userId, Username = username, Email = email });
        }
    }
}
