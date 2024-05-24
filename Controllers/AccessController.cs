using Digital_Wallet.Data;
using Digital_Wallet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Digital_Wallet.Controllers
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account modelLogin)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == modelLogin.Email && u.Password == modelLogin.Password);
            if (user != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            // You can add more claims here as needed
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ValidateMessage"] = "Invalid email or password.";
            return View();
        }

     
    }
}
