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

        // [HttpPost]
        // public async ActionResult<int> CreateClients() 
        // {
            
        // }


        /*
        [HttpPost("api/upload")]
public async Task<IHttpActionResult> Upload()
{
    if (!Request.Content.IsMimeMultipartContent())
        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType); 

    var provider = new MultipartMemoryStreamProvider();
    await Request.Content.ReadAsMultipartAsync(provider);
    foreach (var file in provider.Contents)
    {
        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
        var buffer = await file.ReadAsByteArrayAsync();
        //Do whatever you want with filename and its binary data.
    }

    return Ok();
}
        
        
        */
    }
}