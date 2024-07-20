using LogisticsAPI.Data;
using Microsoft.EntityFrameworkCore;

const string POSTGRE_SQL_CONNECTION = "PostgreSqlConnection";

var builder = WebApplication.CreateBuilder(args);

// Включение использования контроллеров, MVC
builder.Services.AddControllers();

// DI
// builder.Services.AddScoped<IClientAPIRepo, MockClientAPIRepo>(); мок-объекты
builder.Services.AddScoped<IClientAPIRepo, SQLClientApiRepo>(); 

// Добавление контекста БД и строки подключения
builder.Services.AddDbContext<ClientContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString(POSTGRE_SQL_CONNECTION))
);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    // Использование контроллеров в качестве конечных точек 
    _ = endpoints.MapControllers();
});

app.Run();