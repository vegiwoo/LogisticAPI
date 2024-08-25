using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models
{
    public class Client(int serviceId, string name, string phone, string? nick = null)
    {
        [Key, Required]
        public int ServiceId { get; set; } = serviceId;

        [MaxLength(250)]
        public string Name { get; set; } = name;

        public string? Nick { get; set; } = nick;

        [MaxLength(250)]
        public string? Alias { get; set; }

        [Required]
        public string Phone { get; set; } = phone;

        [Required]
        public DateOnly UpdateAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}