using AutoMapper;
using LogisticsAPI.DTOs;
using LogisticsAPI.Models;

namespace LogisticsAPI.Profiles
{
    public class ClientsProfile : Profile
    {
       public ClientsProfile()
       {
            //Source -> Target
            CreateMap<Client, ClientReadDTO>();
            CreateMap<ClientCreateDTO, Client>();
       }
    }
}