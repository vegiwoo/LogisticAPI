using LogisticsAPI.Models;

namespace LogisticsAPI.Data
{
    public class SQLClientApiRepo(ClientContext context) : IClientAPIRepo
    {
        private readonly ClientContext _context = context;

        public void CreateClient(Client client)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(Client cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAllClients() => [.. _context.ClientItems];
        
        public Client? GetClientById(int id) => 
            _context.ClientItems.FirstOrDefault(p => p.ServiceId == id);

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}