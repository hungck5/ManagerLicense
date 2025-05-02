var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
		.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
		{
			policy
				.WithOrigins("https://localhost:3001")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.MapReverseProxy();
app.Run();
