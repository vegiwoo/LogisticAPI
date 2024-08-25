using LogisticsAPI.Data;
using LogisticsAPI.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

const string POSTGRE_SQL_CONNECTION = "PostgreSqlConnection";
const string USER_ID_NAME = "User ID";
const string USER_PASSWORD_NAME = "Password";

var builder = WebApplication.CreateBuilder(args);

//builder.Environment.EnvironmentName = "Development";

// Включение использования контроллеров, MVC
builder.Services.AddControllers();

// DI
// builder.Services.AddScoped<IClientAPIRepo, MockClientAPIRepo>(); мок-объекты
builder.Services.AddScoped<IClientAPIRepo, SQLClientApiRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IFileService, FileService>();

// Добавление контекста БД и строки подключения
var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString(POSTGRE_SQL_CONNECTION))
{
    Username = builder.Configuration[USER_ID_NAME],
    Password = builder.Configuration[USER_PASSWORD_NAME]
};

builder.Services.AddDbContext<ClientContext>(opt => 
    opt.UseNpgsql(npgsqlConnectionStringBuilder.ConnectionString)
);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => 
{
    // Использование контроллеров в качестве конечных точек 
    _ = endpoints.MapControllers();
});

app.Run();