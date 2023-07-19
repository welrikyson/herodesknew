using herodesknew.Application.Tickets.Queries.GetTickets;
using herodesknew.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly GetMembersQueryHandler _getMembersQueryHandler;

        public TicketController(ILogger<TicketController> logger, GetMembersQueryHandler getMembersQueryHandler)
        {
            _logger = logger;
            _getMembersQueryHandler = getMembersQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _getMembersQueryHandler.Handle(new GetTicketsQuery());
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return NotFound();
            }
            
        }
    }
}
