using AuthServer.DB;
using OpenIddict.Abstractions;
using AuthServer.seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenIddict.Client;
using Microsoft.AspNetCore.HttpOverrides;

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
						 .SetEndSessionEndpointUris("/connect/logout")
						 .SetConfigurationEndpointUris("/.well-known/openid-configuration")
       			.SetIntrospectionEndpointUris("/connect/introspect");

			options.SetIssuer(new Uri("https://localhost:7130/auth"));
			options.AllowAuthorizationCodeFlow()
						 .RequireProofKeyForCodeExchange()
						 .AllowRefreshTokenFlow();

			options.AcceptAnonymousClients();
			options.RegisterScopes(
					OpenIddictConstants.Scopes.OpenId,
					OpenIddictConstants.Scopes.Email,
					OpenIddictConstants.Scopes.Profile,
					OpenIddictConstants.Scopes.Roles,
					OpenIddictConstants.Scopes.OfflineAccess,
					"ecommerce_api"
			);

			options.AddEphemeralEncryptionKey()
						 .AddEphemeralSigningKey();

			options.UseAspNetCore()
						 .EnableTokenEndpointPassthrough()
						 .EnableAuthorizationEndpointPassthrough()
						 .EnableEndSessionEndpointPassthrough();
						
		})
		.AddValidation(options =>
		{
			options.UseLocalServer();
			options.UseAspNetCore();
			options.SetIssuer(new Uri("https://localhost:7130/auth"));
		});

builder.Services.AddControllersWithViews();

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
};

// Nếu chạy local, cần allow loopback addresses:
forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();

app.Use(async (context, next) =>
{
	Console.WriteLine($"Request: {context.Request.Method} {context.Request.Scheme} {context.Request.Host.Value} ");
    Console.WriteLine($"Request: {context.Request.PathBase.Value} {context.Request.Path}");
    await next();
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});
app.UseForwardedHeaders(forwardedHeadersOptions);
app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/connect/authorize"), subApp =>
{
	subApp.UseAuthentication();
	subApp.UseAuthorization();
});

app.MapControllers();

await OpenIddictSeeder.SeedAsync(app.Services);
app.Run();