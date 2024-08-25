using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.DTOs
{   
    public class ClientCreateDTO(int serviceId, string name, string phone, string? nick = null)
    {
        [Key, Required]
        public int ServiceId { get; set; } = serviceId;

        [Required, MaxLength(250)]
        public string Name { get; set; } = name;

        public string? Nick { get; set; } = nick;

        [Required]
        public string Phone { get; set; } = phone;
    }
}