using LogisticsAPI.Models;

namespace LogisticsAPI.Data
{
    public class SQLClientApiRepo(ClientContext context) : IClientAPIRepo
    {
        private readonly ClientContext _context = context;

        public void CreateClients(IEnumerable<Client> clients)
        {
            if(!clients.Any()) 
            {
                throw new ArgumentException(null, nameof(clients));
            } 
            else 
            {
                _context.ClientItems.AddRange(clients);
            }
        }

        public void DeleteClient(Client cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAllClients() => [.. _context.ClientItems];
        
        public Client? GetClientById(int id) => 
            _context.ClientItems.FirstOrDefault(p => p.ServiceId == id);

        public bool SaveChanges() => 
            _context.SaveChanges() >= 0;
        

        public void UpdateClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}