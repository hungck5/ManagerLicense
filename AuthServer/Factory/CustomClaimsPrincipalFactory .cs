using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;

namespace AuthServer.Factory
{
  public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser>
  {
      public CustomClaimsPrincipalFactory(
          UserManager<IdentityUser> userManager,
          IOptions<IdentityOptions> optionsAccessor)
          : base(userManager, optionsAccessor)
      {
      }

      public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
      {
        var principal = await base.CreateAsync(user);
        var identity = (ClaimsIdentity)principal.Identity;

        if (!principal.HasClaim(c => c.Type == OpenIddictConstants.Claims.Subject))
        {
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id));
        }

        if (!principal.HasClaim(c => c.Type == OpenIddictConstants.Claims.Email))
        {
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Email, user.Email ?? ""));
        }

        if (!principal.HasClaim(c => c.Type == OpenIddictConstants.Claims.Name))
        {
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Name, user.UserName ?? ""));
        }

          return principal;
      }
  }

}
