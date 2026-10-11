using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace AuthServer.seed;
public static class OpenIddictSeeder
{
  public static async Task SeedAsync(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var ecommerceUrl = new Uri(configuration["Auth:EcommerceUrl"] ?? "https://localhost:7294/");
    var reactUrl = new Uri(configuration["Auth:ReactUrl"] ?? "https://localhost:3001/");

    if (await manager.FindByClientIdAsync("ecommerce_app") is null)
    {
      await manager.CreateAsync(new OpenIddictApplicationDescriptor
      {
        ClientId = "ecommerce_app",
        ClientType = ClientTypes.Public,
        ConsentType = ConsentTypes.Implicit,
        DisplayName = "Ecommerce Web App",
        PostLogoutRedirectUris = { new Uri(ecommerceUrl, "signout-callback-oidc") },
        RedirectUris = { new Uri(ecommerceUrl, "signin-oidc") },
        Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.Endpoints.EndSession,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.ResponseTypes.Code,
                Scopes.OpenId,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Email,
                Permissions.Prefixes.Scope + "ecommerce_api"
            },
        Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
      });
    }

    if (await scopeManager.FindByNameAsync("ecommerce_api") is null)
    {
      await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
      {
        Name = "ecommerce_api",
        Resources = { "resource_server" }
      });
    }

    if (await manager.FindByClientIdAsync("react_client") is null)
        {
            await manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "react_client",
                DisplayName = "React.js App",
                ConsentType = ConsentTypes.Explicit,
                ClientType = ClientTypes.Public,                
                PostLogoutRedirectUris =
                {
                    reactUrl,
                },
                RedirectUris =
                {
                    new Uri(reactUrl, "callback"),
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Token,
                    Permissions.Endpoints.EndSession,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + Scopes.OfflineAccess,
                    Permissions.Prefixes.Scope + "ecommerce_api",
                },
                Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange,
                }
            });
        }
  }

}
