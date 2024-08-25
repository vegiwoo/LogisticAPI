using LogisticsAPI.Models;

namespace LogisticsAPI.Data
{
    public interface IClientAPIRepo
    {
        bool SaveChanges();
        IEnumerable<Client> GetAllClients();
        Client? GetClientById(int id);
        void CreateClients(IEnumerable<Client> clients);
        void UpdateClient(Client client);
        void DeleteClient(Client cmd);
    }
}