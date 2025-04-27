using AuthServer.DB;
using OpenIddict.Abstractions;
using AuthServer.seed;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
  options.UseOpenIddict();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "ecommerce_api"); 
    });
});

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
      options.UseEntityFrameworkCore()
          .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(options =>
    {
      options.SetTokenEndpointUris("/connect/token")
             .SetAuthorizationEndpointUris("/connect/authorize")
             .SetEndSessionEndpointUris("/connect/logout");

      options.AllowAuthorizationCodeFlow()
             .RequireProofKeyForCodeExchange();

      options.RegisterScopes(
          OpenIddictConstants.Scopes.OpenId,
          OpenIddictConstants.Scopes.Email,
          OpenIddictConstants.Scopes.Profile,
          "ecommerce_api"
      );

      options.AddEphemeralEncryptionKey()
             .AddEphemeralSigningKey();

      options.UseAspNetCore()
             .EnableAuthorizationEndpointPassthrough()
             .EnableEndSessionEndpointPassthrough();
    })
    .AddValidation(options => 
    {
      options.UseLocalServer();
      options.UseAspNetCore();
      options.SetIssuer(new Uri("https://localhost:7294/"));
    });
    
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();

app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/connect/authorize"), subApp =>
{
    subApp.UseAuthentication();
    subApp.UseAuthorization();
});

app.MapControllers();

await OpenIddictSeeder.SeedAsync(app.Services);
app.Run();