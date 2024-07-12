using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models
{
    public class Client(int id, string name, string alias, string phone, Messenger? messenger = null)
    {
        [Key, Required]
        public int ID { get; set; } = id;

        [Required, MaxLength(250)]
        public string Name { get; set; } = name;

        [MaxLength(250)]
        public string Alias { get; set; } = alias;

        [Required]
        public string Phone { get; set; } = phone;

        public List<Messenger>? Messengers { get; set; } = 
            messenger != null ? [messenger] : null;

        [Required]
        public DateOnly UpdateAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}