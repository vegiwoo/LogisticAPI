namespace LogisticsAPI.DTOs
{
    public class ClientReadDTO(int serviceId, string name, string alias, string phone)
    {
        public int ServiceId { get; set; } = serviceId;

        public string Name { get; set; } = name;

        public string Alias { get; set; } = alias;

        public string Phone { get; set; } = phone;
    }
}