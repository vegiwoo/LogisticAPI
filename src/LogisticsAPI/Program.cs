var builder = WebApplication.CreateBuilder(args);

// Включение использования контроллеров, MVC
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    // Использование контроллеров в качестве конечных точек 
    _ = endpoints.MapControllers();
});

//app.MapGet("/", () => "Hello World!");
app.Run();