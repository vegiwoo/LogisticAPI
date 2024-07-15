using LogisticsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Включение использования контроллеров, MVC
builder.Services.AddControllers();

// DI
builder.Services.AddScoped<IClientAPIRepo, MockClientAPIRepo>();


var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    // Использование контроллеров в качестве конечных точек 
    _ = endpoints.MapControllers();
});

app.Run();