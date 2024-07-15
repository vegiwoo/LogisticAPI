using LogisticsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAPI.Data
{
    // Представление базы данных
    public class ClientContext(DbContextOptions<ClientContext> options) : DbContext(options)
    {
        // Представление таблицы в базе данных 
        public DbSet<Client> ClientItems {get; set;}
    }
}