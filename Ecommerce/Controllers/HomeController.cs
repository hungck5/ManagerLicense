using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authentication;

namespace Ecommerce.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> Profile()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync();

        if (!authenticateResult.Succeeded)
        {
            return Unauthorized();
        }

        // Lấy access token
        var accessToken = authenticateResult.Properties.GetTokenValue("access_token");

        // Lấy id token
        var idToken = authenticateResult.Properties.GetTokenValue("id_token");

        // Lấy refresh token (nếu có)
        var refreshToken = authenticateResult.Properties.GetTokenValue("refresh_token");

        var userClaims = authenticateResult.Principal.Claims;
         var userName = userClaims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
    var userEmail = userClaims.FirstOrDefault(c => c.Type == "email")?.Value;
    var userPicture = userClaims.FirstOrDefault(c => c.Type == "picture")?.Value;

        // In ra xem thử
        Console.WriteLine($"Access Token: {accessToken}");
        Console.WriteLine($"ID Token: {idToken}");
        Console.WriteLine($"Refresh Token: {refreshToken}");
        Console.WriteLine($"User Name: {userName}");
        Console.WriteLine($"User Email: {userEmail}");
        Console.WriteLine($"User Picture: {userPicture}");

        return View();
    }

}
