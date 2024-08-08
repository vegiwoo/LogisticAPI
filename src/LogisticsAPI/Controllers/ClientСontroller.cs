using AutoMapper;
using LogisticsAPI.Data;
using LogisticsAPI.DTOs;
using LogisticsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsСontroller(IClientAPIRepo repository, IMapper mapper) : ControllerBase
    {
        private readonly IClientAPIRepo _repository = repository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public ActionResult<IEnumerable<ClientReadDTO>> GetAllClients() => 
            Ok(_mapper.Map<IEnumerable<ClientReadDTO>>(_repository.GetAllClients()));
        
        [HttpGet("{id}")]
        public ActionResult<ClientReadDTO> GetClientById(int id) 
        {
            var clientItem = _repository.GetClientById(id);
            return clientItem is null ? NotFound() : Ok(_mapper.Map<ClientReadDTO>(clientItem));
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