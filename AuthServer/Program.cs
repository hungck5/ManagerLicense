using AuthServer.DB;
using OpenIddict.Abstractions;
using AuthServer.seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using AuthServer.Factory;

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
		options.SetIssuer(new Uri("https://localhost:7130/"));
		options.SetTokenEndpointUris("/connect/token")
			.SetAuthorizationEndpointUris("/connect/authorize")
			.SetEndSessionEndpointUris("/connect/logout")
			.SetConfigurationEndpointUris("/.well-known/openid-configuration")
			.SetIntrospectionEndpointUris("/connect/introspect");

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
});

builder.Services.AddControllersWithViews();

var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
};

forwardedHeadersOptions.KnownNetworks.Clear();
forwardedHeadersOptions.KnownProxies.Clear();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
  builder.Services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, CustomClaimsPrincipalFactory>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();

app.UseForwardedHeaders(forwardedHeadersOptions);
app.UseWhen(ctx => !ctx.Request.Path.StartsWithSegments("/connect/authorize"), subApp =>
{
	subApp.UseAuthentication();
	subApp.UseAuthorization();
});

app.MapControllers();
await OpenIddictSeeder.SeedAsync(app.Services);
await SeedUser.SeedAdminUser(app.Services);

app.Run();