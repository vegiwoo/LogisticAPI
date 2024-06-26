using System.ComponentModel.DataAnnotations;

namespace LogisticsAPI.Models
{
    public class Client
    {
        [Key, Required]
        public int ID {get;set;}

        [Required, MaxLength(250)]
        public string Name {get;set;}

        [MaxLength(250)]
        public string Alias {get;set;}

        [Required]
        public string Phone {get;set;}

        [Required]
        public DateOnly UpdateAt {get;set;}
}