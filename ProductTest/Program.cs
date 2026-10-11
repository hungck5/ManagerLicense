var builder = WebApplication.CreateBuilder(args);
if (string.IsNullOrEmpty(builder.Configuration["ASPNETCORE_HTTP_PORTS"]) &&
    string.IsNullOrEmpty(builder.Configuration["ASPNETCORE_URLS"]))
    builder.WebHost.UseUrls("http://localhost:5001");

var app = builder.Build();

app.MapGet("/products", () => "Product list from ProductService");

app.Run();
