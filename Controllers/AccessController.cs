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
                    new Claim(ClaimTypes.Name, user.FirstName), // Claim for first name
                    new Claim(ClaimTypes.Surname, user.LastName), 
                    // You can add more claims here as needed
                };

                if (user.Email == "Gnshr@admin.com") // Add a condition for the specific user
                {
                    claims.Add(new Claim(ClaimTypes.Name, "specificusername")); // Add a claim for the specific user
                }

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
