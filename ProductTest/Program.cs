var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5001");

var app = builder.Build();

app.MapGet("/products", () => "Product list from ProductService");

app.Run();
