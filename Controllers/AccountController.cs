using Microsoft.AspNetCore.Mvc;
using AgriStreamWeb.Data;
using AgriStreamWeb.Models;
using System.Linq;

namespace AgriStreamWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register() => View();

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    // Simple hash or string storage for testing setup
                    PasswordHash = model.Password,
                    Role = model.Role
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: Account/Login
        public IActionResult Login() => View();

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.Password);
                if (user != null)
                {
                    // Match found! Navigate to home page
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
    }
}
