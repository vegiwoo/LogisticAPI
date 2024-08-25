using System.Diagnostics;
using AutoMapper;
using LogisticsAPI.Data;
using LogisticsAPI.DTOs;
using LogisticsAPI.Models;
using LogisticsAPI.Services.FileService;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace LogisticsAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsСontroller(IClientAPIRepo repository, IMapper mapper, IFileService fileService) : ControllerBase
    {
        private readonly IClientAPIRepo _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileService _fileService = fileService;

        [HttpGet]
        public ActionResult<IEnumerable<ClientReadDTO>> GetAllClients() => 
            Ok(_mapper.Map<IEnumerable<ClientReadDTO>>(_repository.GetAllClients()));
        
        [HttpGet("{id}")]
        public ActionResult<ClientReadDTO> GetClientById(int id) 
        {
            var clientItem = _repository.GetClientById(id);
            return clientItem is null ? NotFound() : Ok(_mapper.Map<ClientReadDTO>(clientItem));
        }

        [HttpPost]
        public async Task<ActionResult<long>> CreateClients(IFormFile data) 
        {
            if (data == null || data.Length <= 0)
                return BadRequest("File cannot be empty");

            var currentExtension = Path.GetExtension(data.FileName);

            if(_fileService.CheckFileExtension(FileType.Excel, in currentExtension))
                return BadRequest($"'{currentExtension}'extension is not suitable for this file.");

            if(_fileService.CheckFileName(FileContext.СlientsSKUs, data.FileName))
                return BadRequest($"Name '{data.FileName}' is not suitable for this file.");

            await Task.Delay(1000);

            return Ok(data.Length);
        }

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