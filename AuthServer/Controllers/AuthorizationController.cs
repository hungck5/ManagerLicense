using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore;
using OpenIddict.Server.AspNetCore;
using Microsoft.AspNetCore.Authorization;

[Route("connect")]
[AllowAnonymous]
public class AuthorizationController : Controller
{
    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        Console.WriteLine("Authorize request received.");
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Request.Path + Request.QueryString
            });
        }

        var claims = new List<Claim>
        {
            new(OpenIddictConstants.Claims.Subject, User.Identity.Name ?? "user123"),
            new(OpenIddictConstants.Claims.Email, "test@example.com"),
            new(OpenIddictConstants.Claims.Name, "John Doe")
        };

        var identity = new ClaimsIdentity(claims,
            TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.Name,
            OpenIddictConstants.Claims.Role);

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(request.GetScopes());
        principal.SetResources("resource_server");

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    // [HttpPost("token")]
    // public async Task<IActionResult> Exchange()
    // { 
    //     Console.WriteLine("Exchange token request received.");
    //     var request = HttpContext.GetOpenIddictServerRequest() ??
    //                 throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

    //     if (request.IsAuthorizationCodeGrantType())
    //     {
    //         var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

    //         var claims = new List<Claim>
    //         {
    //             new(OpenIddictConstants.Claims.Subject, authenticateResult.Principal.Identity?.Name ?? "user123"),
    //             new(OpenIddictConstants.Claims.Email, "test@example.com"),
    //             new(OpenIddictConstants.Claims.Name, "John Doe")
    //         };

    //         var identity = new ClaimsIdentity(claims,
    //             TokenValidationParameters.DefaultAuthenticationType,
    //             OpenIddictConstants.Claims.Name,
    //             OpenIddictConstants.Claims.Role);

    //         var principal = new ClaimsPrincipal(identity);

    //         principal.SetScopes(request.GetScopes());
    //         principal.SetResources("resource_server");

    //         return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    //     }

    //     throw new InvalidOperationException("The specified grant type is not supported.");
    // }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");

        var request = HttpContext.GetOpenIddictServerRequest();
        var postLogoutRedirectUri = request?.PostLogoutRedirectUri;

        if (!string.IsNullOrEmpty(postLogoutRedirectUri))
        {
            return Redirect(postLogoutRedirectUri);
        }

        return Redirect("~/");
    }

}
