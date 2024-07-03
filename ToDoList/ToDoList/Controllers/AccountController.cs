using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Data;
using ToDoList.Models;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _context.Users
            .SingleOrDefaultAsync(user => user.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Пользователь с таким Email уже существует");
                return View(model);
            }

            var user = new UserModel
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }
       
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Пользователь с таким Email не найден");
                return View(model);
            }

            if (user.Password != model.Password)
            {
                ModelState.AddModelError("Password", "Неверный пароль");
                return View(model);
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

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Account");
    }
}
