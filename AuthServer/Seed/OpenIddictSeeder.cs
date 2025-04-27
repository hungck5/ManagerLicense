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

    // Tạo Client cho EcommerceApp
    if (await manager.FindByClientIdAsync("ecommerce_app") is null)
    {
      await manager.CreateAsync(new OpenIddictApplicationDescriptor
      {
        ClientId = "ecommerce_app",
        ConsentType = ConsentTypes.Implicit,
        DisplayName = "Ecommerce Web App",
        PostLogoutRedirectUris = { new Uri("https://localhost:7294/signout-callback-oidc") },
        RedirectUris = { new Uri("https://localhost:7294/signin-oidc") },
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
                "ecommerce_api"
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
  }

}
