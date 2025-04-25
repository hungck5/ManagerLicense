using AuthServer.DB;
using OpenIddict.Abstractions;
using AuthServer.seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
  options.UseOpenIddict();
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
             .EnableTokenEndpointPassthrough()
             .EnableEndSessionEndpointPassthrough()
             .EnableStatusCodePagesIntegration();
    })
    .AddValidation(options =>
    {
      options.UseLocalServer();
      options.UseAspNetCore();
    });

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Hello World!").RequireAuthorization("ApiScope");

await OpenIddictSeeder.SeedAsync(app.Services);
app.Run();