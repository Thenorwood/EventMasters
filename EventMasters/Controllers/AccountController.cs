using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventMasters.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {

            return View();
        }

        //POST: login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(string username, string password, string returnUrl)
        {

            if (username == _configuration["Myusername"] && password == _configuration["Mypassword"])
            {
                // Create a list of claims identifying the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, "admin"), // unique ID
                    new Claim(ClaimTypes.Name, "Administrator"), // human readable name
                            
                };

                // Create the identity from the claims
                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign-in the user with the cookie authentication scheme
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));


                if (!string.IsNullOrEmpty(returnUrl))
                {
                    // If user came from other page, re-direct back to that URL
                    return Redirect(returnUrl);
                }
                else
                {
                    // Re-direct to home page
                    return RedirectToAction("Index", "Home");
                }

            }

            ViewBag.ErrorMessage = "Invalid username or password.";

            return View();
        }


        //get:logout
        public IActionResult Logout()
        {
            return View();
        }

        // POST: /Account/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirmed()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Re-direct user to login page after logout
            return RedirectToAction("Login", "Account");
        }
    }
}
