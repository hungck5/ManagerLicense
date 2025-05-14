using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore;
using OpenIddict.Server.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[Route("connect")]
[AllowAnonymous]
public class AuthorizationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public AuthorizationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet("authorize")]
    public async Task<IActionResult> AuthorizeAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Request.Path + Request.QueryString
            });
        }

        var user = await _userManager.GetUserAsync(User);
        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        principal.SetScopes(request.GetScopes());
        principal.SetResources("resource_server");

        principal.SetDestinations(claim =>
        {
            if (claim.Type == OpenIddictConstants.Claims.Email)
                return new[] { OpenIddictConstants.Destinations.IdentityToken, OpenIddictConstants.Destinations.AccessToken };
            if (claim.Type == OpenIddictConstants.Claims.Name)
                return new[] { OpenIddictConstants.Destinations.IdentityToken, OpenIddictConstants.Destinations.AccessToken };
            return new[] { OpenIddictConstants.Destinations.AccessToken };
        });

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("token")]
    public async Task<IActionResult> Exchange()
    { 
        Console.WriteLine("Exchange token request received.");
        var request = HttpContext.GetOpenIddictServerRequest() ??
                    throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsAuthorizationCodeGrantType())
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            var user = await _userManager.GetUserAsync(authenticateResult.Principal);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            principal.SetScopes(request.GetScopes());
            principal.SetResources("resource_server");

            principal.SetDestinations(claim =>
            {
                if (claim.Type == OpenIddictConstants.Claims.Email)
                    return new[] { OpenIddictConstants.Destinations.IdentityToken, OpenIddictConstants.Destinations.AccessToken };
                if (claim.Type == OpenIddictConstants.Claims.Name)
                    return new[] { OpenIddictConstants.Destinations.IdentityToken, OpenIddictConstants.Destinations.AccessToken };
                return new[] { OpenIddictConstants.Destinations.AccessToken };
            });

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new InvalidOperationException("The specified grant type is not supported.");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        var request = HttpContext.GetOpenIddictServerRequest();
        var postLogoutRedirectUri = request?.PostLogoutRedirectUri;

        if (!string.IsNullOrEmpty(postLogoutRedirectUri))
        {
            return Redirect(postLogoutRedirectUri);
        }

        return Redirect("~/");
    }
}
