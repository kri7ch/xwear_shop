using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using XwearShopAPI.Model;

namespace XwearShopAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DbExwearContext _db;

        public UsersController(DbExwearContext db)
        {
            _db = db;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
                return BadRequest(new { message = "Email и пароль обязательны" });

            var exists = await _db.Users.AnyAsync(u => u.Email == input.Email);
            if (exists)
                return Conflict(new { message = "Пользователь с таким email уже существует" });

            var user = new User
            {
                Email = input.Email,
                Password = input.Password,
                Name = null,
                Phone = null
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Created($"/api/users/{user.Id}", new { id = user.Id, email = user.Email, name = user.Name, phone = user.Phone });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
                return BadRequest(new { message = "Email и пароль обязательны" });

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == input.Email);
            if (user == null || user.Password != input.Password)
                return Unauthorized(new { message = "Неверный email или пароль" });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? user.Email),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2),
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            return Ok(new { message = "ok" });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var user = await _db.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized();

            return Ok(new { id = user.Id, email = user.Email, name = user.Name, phone = user.Phone });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "logged_out" });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}