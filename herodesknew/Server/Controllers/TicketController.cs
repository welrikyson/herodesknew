using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;

        public TicketController(ILogger<TicketController> logger)
        {
            _logger = logger;
        }
    }
}
