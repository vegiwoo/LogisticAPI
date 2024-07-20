using LogisticsAPI.Models;

namespace LogisticsAPI.Data
{
    public class MockClientAPIRepo : IClientAPIRepo
    {
        private readonly IEnumerable<Client> FakeClientsData =      
        [
            new Client(0, "Петр Иванов", "Иванов Петр Михайлович", "+79999955820"),
            new Client(1, "Иван Петров", "Петров Иван Семенович", "+79453213468"/*, new Messenger(MessengerType.Telegram, "@testTlgName02")*/),
            new Client(2, "Борис Николаев", "Николаев Борис Григорьевич", "+79453213468"/*, new Messenger(MessengerType.Telegram, "@testTlgName02")*/),
        ];

        public void CreateClient(Client client)
        {
            throw new NotImplementedException();
        }

        public void DeleteClient(Client cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FakeClientsData;
        }

        public Client GetClientById(int id)
        {
           return FakeClientsData.First();
        }

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