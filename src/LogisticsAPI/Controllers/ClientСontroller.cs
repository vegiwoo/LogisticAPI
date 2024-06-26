using Microsoft.AspNetCore.Mvc;

namespace LogisticsAPI.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsСontroller : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() => 
            new string[] {"this", "is", "hard", "coded"};
        
    }
}