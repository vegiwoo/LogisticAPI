using LogisticsAPI.Data;
using LogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsСontroller(IClientAPIRepo repository) : ControllerBase
    {
        private readonly IClientAPIRepo _repository = repository;

        [HttpGet]
        public ActionResult<IEnumerable<Client>> GetAllClients()  
        {
            var clientItems = _repository.GetAllClients();
            return Ok(clientItems);
        }

        [HttpGet("{id}")]
        public ActionResult<Client> GetClientById(int id) 
        {
            var clientItem = _repository.GetClientById(id);
            return clientItem is null ? NotFound() : Ok(clientItem);
        }
    }
}