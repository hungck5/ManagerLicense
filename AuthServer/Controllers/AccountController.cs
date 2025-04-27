
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

[Route("account")]
public class AccountController : Controller
{
    [HttpGet("login")]
    public IActionResult Login(string returnUrl)
    {
        // Hiện form login, hoặc auto login demo
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginPost(string username, string password, string returnUrl)
    {
        // Check username/password, ví dụ hardcoded trước cho nhanh
        if (username == "admin" && password == "123")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, "admin@example.com")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(returnUrl ?? "/");
        }

        return Unauthorized();
    }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Logout()
    // {
    //     await HttpContext.SignOutAsync("Cookies");
    //     await HttpContext.SignOutAsync("oidc");

    //     return RedirectToAction("Index", "Home");
    // }
}
