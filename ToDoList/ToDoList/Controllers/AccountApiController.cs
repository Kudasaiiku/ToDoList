using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Account/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users
                    .SingleOrDefaultAsync(user => user.Email == model.Email);

                if (existingUser != null)
                {
                    return BadRequest("Пользователь с таким Email уже существует");
                }

                var user = new UserModel
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return BadRequest(ModelState);
        }

        // POST: api/Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || user.Password != model.Password)
                {
                    return Unauthorized("Неверный Email или пароль");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return Ok();
            }

            return BadRequest(ModelState);
        }

        // POST: api/Account/Logout
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
    }
}
