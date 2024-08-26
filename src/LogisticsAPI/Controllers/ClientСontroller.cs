using System.Diagnostics;
using AutoMapper;
using LogisticsAPI.Data;
using LogisticsAPI.DTOs;
using LogisticsAPI.Models;
using LogisticsAPI.Services.ExcelService;
using LogisticsAPI.Services.ExcelService.Items;
using LogisticsAPI.Services.FileService;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace LogisticsAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsСontroller(IClientAPIRepo repository, IMapper mapper, IFileService fileService, IExcelService excelService) : ControllerBase
    {
        private readonly IClientAPIRepo _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileService _fileService = fileService;
        private readonly IExcelService _excelService = excelService;

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
        public async Task<ActionResult<int>> CreateClients(IFormFile data) 
        {
            // Checking for useful data 
            if (data == null || data.Length <= 0)
                return BadRequest("File cannot be empty");

            // Check file name and extension
            var currentExtension = Path.GetExtension(data.FileName);
            if(_fileService.CheckFileExtension(FileType.Excel, in currentExtension))
                return BadRequest($"'{currentExtension}'extension is not suitable for this file.");
            if(_fileService.CheckFileName(FileContext.СlientsSKUs, data.FileName))
                return BadRequest($"Name '{data.FileName}' is not suitable for this file.");

            // Copy data in MemoryStream
<<<<<<< HEAD

=======
>>>>>>> 35b6ad5 (Implementation of getting a range of rows when parsing an Excel sheet)
            using var stream = new MemoryStream();
            CancellationToken cancellationToken = new();
            await data.CopyToAsync(stream, cancellationToken);

            // Create ExcelPackage and find ExcelWorksheet
            using var package = new ExcelPackage(stream);
            var sheetName = _excelService.DataColumnsForParsing[FileContext.СlientsSKUs].worksheetName;
            if (!_excelService.GetWorksheetByName(in package, sheetName, out ExcelWorksheet? excelWorksheet))
                return BadRequest($"There is no Excel sheet named {sheetName} in provided file.");

<<<<<<< HEAD

            // Getting ranges from an Excel sheet
            List<RangeSourceReportRows>? dataColumnsForParsing = _excelService.DataColumnsForParsing
                .SingleOrDefault(el => el.Key == FileContext.СlientsSKUs).Value.ranges;

=======
            // Getting ranges from an Excel sheet
            List<RangeSourceReportRows>? dataColumnsForParsing = _excelService.DataColumnsForParsing
                .SingleOrDefault(el => el.Key == FileContext.СlientsSKUs).Value.ranges;

>>>>>>> 35b6ad5 (Implementation of getting a range of rows when parsing an Excel sheet)
            if(dataColumnsForParsing is null)
                return BadRequest($"No prototype for parsing (key '{FileContext.СlientsSKUs}').");

            _excelService.GetRangesRowsFromExcelWorksheet(in excelWorksheet!, ref dataColumnsForParsing);

            // TODO: Сохранить dataColumnsForParsing обратно в словарь.

            // Getting raw data from Excel sheet ranges

            return Ok(dataColumnsForParsing.First().RangeRowIndexes?.First());
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