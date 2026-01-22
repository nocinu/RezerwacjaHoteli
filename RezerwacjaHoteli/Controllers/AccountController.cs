using Microsoft.AspNetCore.Mvc;
using RezerwacjaHoteli.Data;
using RezerwacjaHoteli.Models;
using System.Security.Cryptography;
using System.Text;

namespace RezerwacjaHoteli.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelReservationDbContext _context;

        public AccountController(HotelReservationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // sprawdzamy czy email jest zajęty
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email już istnieje w systemie");
                    return View(model);
                }

                model.Role = "Guest";
                model.CreatedDate = DateTime.Now;

                _context.Users.Add(model);
                _context.SaveChanges();

                HttpContext.Session.SetString("UserId", model.Id.ToString());
                HttpContext.Session.SetString("UserRole", model.Role);
                HttpContext.Session.SetString("UserEmail", model.Email);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user != null && user.PasswordHash == password)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserEmail", user.Email);

                user.LastLoginDate = DateTime.Now;
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Nieprawidłowy email lub hasło");
            return View();
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
