using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
var allowHttp = builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("Auth:AllowHttp");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
  options.DefaultScheme = "Cookies";
  options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
  options.Cookie.HttpOnly = true;
  options.Cookie.SecurePolicy = allowHttp ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
  options.SlidingExpiration = true;
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
  options.Authority = builder.Configuration["Auth:Authority"] ?? "https://localhost:7130/auth";
  if (builder.Configuration["Auth:InternalAuthority"] is { } internalAuthority)
    options.BackchannelHttpHandler = new DockerBackchannelHandler(new Uri(options.Authority), new Uri(internalAuthority));
  options.ClientId = "ecommerce_app";
  options.ResponseType = "code";
  options.UsePkce = true;
  options.SaveTokens = true;
  options.CallbackPath = "/signin-oidc";
  options.SignedOutCallbackPath = "/signout-callback-oidc";
  options.SignedOutRedirectUri = "/";

  options.Scope.Add("ecommerce_api");
  options.Scope.Add("openid");
  options.Scope.Add("profile");
  options.Scope.Add("email");
  options.GetClaimsFromUserInfoEndpoint = false;
  options.RequireHttpsMetadata = !allowHttp;
  if (allowHttp)
  {
    options.ResponseMode = "query";
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
    options.NonceCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.NonceCookie.SameSite = SameSiteMode.Lax;
  }

  options.Events = new OpenIdConnectEvents
  {
      OnSignedOutCallbackRedirect = context =>
      {
          Console.WriteLine("Redirecting after signout callback.");
          context.Response.Redirect("/");
          return Task.CompletedTask;
      }
  };
});

var app = builder.Build();
if (app.Configuration.GetValue<bool>("Database:Initialize"))
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "slug",
    pattern: "{*slug}",
    defaults: new { controller = "Page", action = "RenderPage" });

app.Run();
