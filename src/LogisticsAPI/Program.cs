using LogisticsAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Включение использования контроллеров, MVC
builder.Services.AddControllers();

// DI
builder.Services.AddScoped<IClientAPIRepo, MockClientAPIRepo>();
/*
    AddTransient: Служба создается каждый раз, когда она запрашивается из Service Container.
    AddScoped: Служба создается один раз для каждого клиентского запроса (подключения). 
    AddSingleton: Сервис создается один раз.
*/


var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    // Использование контроллеров в качестве конечных точек 
    _ = endpoints.MapControllers();
});

//app.MapGet("/", () => "Hello World!");
app.Run();