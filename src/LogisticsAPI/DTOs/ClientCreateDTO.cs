using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.DTOs
{   
    public class ClientCreateDTO(int serviceId, string name, string alias, string phone)
    {
        [Key, Required]
        public int ServiceId { get; set; } = serviceId;

        [Required, MaxLength(250)]
        public string Name { get; set; } = name;

        [MaxLength(250)]
        public string Alias { get; set; } = alias;

        [Required]
        public string Phone { get; set; } = phone;
    }
}